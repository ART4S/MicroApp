using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;

namespace EventBus.RabbitMQ;

public class RabbitMQEventBus : IEventBus, IDisposable
{
    const string BrokerName = "PlayingWithDocker";

    private readonly ILogger _logger;
    private readonly IServiceProvider _services;
    private readonly RabbitMQSettings _settings;
    private readonly IConnection _connection;
    private IModel _channel;
    private readonly Dictionary<string, SubscriptionInfo> _events = new();

    public RabbitMQEventBus(ILogger<RabbitMQEventBus> logger, IServiceProvider services, RabbitMQSettings settings)
    {
        _logger = logger;
        _services = services;
        _settings = settings;
        _connection = CreateConnection();
        _channel = CreateChannel();
    }

    private IConnection CreateConnection()
    {
        ConnectionFactory factory = new() 
        { 
            HostName = _settings.HostName, 
            UserName = _settings.UserName, 
            Password = _settings.Password 
        };

        return Policy.Handle<BrokerUnreachableException>().WaitAndRetry(
            retryCount: _settings.Retries,
            sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(Math.Pow(2, attempt)), 
            onRetry:(exception, _, attempt, _) =>
            {
                _logger.LogError(
                    exception, 
                    "Error while establishing rabbitmq connection on attempt {Attempt} of {Retries}", 
                    attempt, _settings.Retries);
            })
            .Execute(() => factory.CreateConnection());
    }

    private IModel CreateChannel()
    {
        IModel channel = _connection.CreateModel();

        channel.ExchangeDeclare(
            exchange: BrokerName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: null);

        channel.QueueDeclare(
            queue: _settings.ClientName, 
            durable: true, 
            exclusive: false, 
            autoDelete: false);

        channel.CallbackException += (s, a) =>
        {
            // TODO: log
            _logger.LogError(a.Exception, "Error in RabbitMQ");
            _logger.LogWarning("Recreating RabbitMQ channel");

            _channel.Dispose();
            _channel = CreateChannel();
        };

        EventingBasicConsumer consumer = new(channel);

        consumer.Received += Consumer_Recievd;

        channel.BasicConsume(queue: _settings.ClientName, autoAck: false, consumer);

        return channel;
    }

    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        Policy.Handle<BrokerUnreachableException>().Or<SocketException>().WaitAndRetry(
            retryCount: _settings.Retries,
            sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            onRetry: (exception, _) =>
            {
                // TODO: log
            })
            .Execute(() =>
            {
                _channel.BasicPublish(
                    exchange: BrokerName,
                    routingKey: GetEventName<TEvent>(),
                    basicProperties: default,
                    body: JsonSerializer.SerializeToUtf8Bytes(@event));
            });
    }

    public void Subscribe<TEvent, TEventHandler>()
        where TEvent : IEvent
        where TEventHandler : IEventHandler<TEvent>
    {
        string eventName = GetEventName<TEvent>();

        if (!_events.TryGetValue(eventName, out var eventInfo))
        {
            _channel.QueueBind(
                queue: _settings.ClientName, 
                exchange: BrokerName,
                routingKey: eventName);

            eventInfo = new(EventType: typeof(TEvent));

            _events.TryAdd(eventName, eventInfo);
        }

        Type handlerType = typeof(TEventHandler);

        if (eventInfo.Handlers.Any(x => x.EventHandlerType == handlerType))
        {
            _logger.LogWarning("Event handler {EventHandler} for event {Event} already exists", handlerType.FullName, typeof(TEvent).FullName);
            return;
        }

        EventingBasicConsumer consumer = new(_channel);

        consumer.Received += Consumer_Recievd;

        string tag = _channel.BasicConsume(queue: _settings.ClientName, autoAck: false, consumer: consumer);

        EventHandlerInfo handler = new(tag, handlerType);

        eventInfo.Handlers.Add(handler);
    }

    private void Consumer_Recievd(object? sender, BasicDeliverEventArgs args)
    {
        string eventName = args.RoutingKey;

        try
        {
            HandleEvent(eventName, args.Body.ToArray());
        }
        catch(JsonException ex)
        {
            // TODO: log
        }
        catch (NotSupportedException ex)
        {
            // TODO: log
        }
        catch(Exception ex)
        {
            // TODO: log
        }
        finally
        {
            _channel.BasicAck(args.DeliveryTag, multiple: false);
        }
    }

    private void HandleEvent(string eventName, byte[] body)
    {
        if (!_events.TryGetValue(eventName, out var eventInfo))
        {
            // TODO: log
            return;
        }

        using IServiceScope scope = _services.CreateScope();

        object @event = JsonSerializer.Deserialize(body, eventInfo.EventType);

        foreach (var handlerInfo in eventInfo.Handlers)
        {
            var handler = _services.GetRequiredService(handlerInfo.EventHandlerType);

            handlerInfo.EventHandlerType
                .GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public)
                .Invoke(handler, new object[] { @event });
        }
    }

    public void Unsubscribe<TEvent, TEventHandler>()
        where TEvent : IEvent
        where TEventHandler : IEventHandler<TEvent>
    {
        string eventName = GetEventName<TEvent>();

        if (!_events.TryGetValue(eventName, out var eventInfo))
        {
            _logger.LogWarning("There are no subscribtions of {Event} yet", typeof(TEvent).Name);
            return;
        }

        foreach (var handlerInfo in eventInfo.Handlers.ToList())
        {
            if (handlerInfo.EventHandlerType == typeof(TEventHandler))
            {
                _channel.BasicCancel(handlerInfo.Tag);
                eventInfo.Handlers.Remove(handlerInfo);
            }
        }

        if (eventInfo.Handlers.Count == 0)
        {
            _channel.QueueBind(queue: _settings.ClientName, exchange: BrokerName, routingKey: eventName);
            _events.Remove(eventName);
            _logger.LogWarning("Event {Event} has no handlers", eventName);
        }
    }

    private string GetEventName<TEvent>() => typeof(TEvent).Name!;

    public void Dispose()
    {
        _events.Clear();
        _channel.Dispose();
        _connection.Dispose();
    }
}

record SubscriptionInfo(Type EventType)
{
    public List<EventHandlerInfo> Handlers { get; } = new();
}

record EventHandlerInfo(string Tag, Type EventHandlerType);
