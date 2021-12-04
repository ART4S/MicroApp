using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Mime;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace EventBus.RabbitMQ;

public class RabbitMQEventBus : IEventBus, IDisposable
{
    const string BROKER_NAME = "MicroApp";

    private readonly ILogger _logger;
    private readonly IServiceProvider _services;
    private readonly RabbitMQSettings _settings;
    private readonly IConnection _connection;
    private IModel _subscriptionChannel;
    private readonly Dictionary<string, SubscriptionInfo> _events = new();

    public RabbitMQEventBus(ILogger<RabbitMQEventBus> logger, IServiceProvider services, RabbitMQSettings settings)
    {
        _logger = logger;
        _services = services;
        _settings = settings;
        _connection = CreateConnection();
        _subscriptionChannel = CreateSubscriptionChannel();
    }

    private IConnection CreateConnection()
    {
        ConnectionFactory factory = new() 
        { 
            HostName = _settings.HostName, 
            UserName = _settings.UserName, 
            Password = _settings.Password,
            ClientProvidedName = _settings.ClientName,
            DispatchConsumersAsync = true,
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

    private IModel CreateSubscriptionChannel()
    {
        IModel channel;

        try
        {
            channel = _connection.CreateModel();
        }
        catch(Exception ex)
        {
            // TODO: log
            throw;
        }

        channel.ExchangeDeclare(
            exchange: BROKER_NAME,
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

            _subscriptionChannel?.Dispose();
            _subscriptionChannel = CreateSubscriptionChannel();
        };

        AsyncEventingBasicConsumer consumer = new(channel);

        consumer.Received += Consumer_Recievd;

        channel.BasicConsume(queue: _settings.ClientName, autoAck: false, consumer);

        return channel;
    }

    public void Publish(IEvent @event)
    {
        var policy = Policy.Handle<BrokerUnreachableException>().Or<SocketException>().WaitAndRetry(
            retryCount: _settings.Retries,
            sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            onRetry: (exception, _) =>
            {
                // TODO: log
            });

        // TODO: log

        using (var channel = _connection.CreateModel())
        {
            channel.ExchangeDeclare(
                exchange: BROKER_NAME,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null);

            // TODO: log

            policy.Execute(() =>
            {
                IBasicProperties props = _subscriptionChannel.CreateBasicProperties();

                props.DeliveryMode = 2;
                props.ContentType = MediaTypeNames.Application.Json;

                _subscriptionChannel.BasicPublish(
                    exchange: BROKER_NAME,
                    routingKey: GetEventName(@event),
                    basicProperties: props,
                    body: JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType()));
            });
        }
    }

    public void Subscribe<TEvent, TEventHandler>()
        where TEvent : IEvent
        where TEventHandler : IEventHandler<TEvent>
    {
        string eventName = GetEventName<TEvent>();

        if (!_events.TryGetValue(eventName, out var eventInfo))
        {
            _subscriptionChannel.QueueBind(
                queue: _settings.ClientName, 
                exchange: BROKER_NAME,
                routingKey: eventName);

            eventInfo = new(EventType: typeof(TEvent));

            _events.TryAdd(eventName, eventInfo);
        }

        Type handlerType = typeof(TEventHandler);

        if (eventInfo.Handlers.Any(x => x.EventHandlerType == handlerType))
        {
            _logger.LogWarning("Event handler {EventHandler} for event {Event} already exists", handlerType.Name, GetEventName<TEvent>());
            return;
        }

        AsyncEventingBasicConsumer consumer = new(_subscriptionChannel);

        consumer.Received += Consumer_Recievd;

        string tag = _subscriptionChannel.BasicConsume(queue: _settings.ClientName, autoAck: false, consumer: consumer);

        EventHandlerInfo handler = new(tag, handlerType);

        eventInfo.Handlers.Add(handler);
    }

    private async Task Consumer_Recievd(object? sender, BasicDeliverEventArgs args)
    {
        string eventName = args.RoutingKey;

        try
        {
            await HandleEvent(eventName, args.Body.ToArray());
        }
        catch(JsonException ex)
        {
            // TODO: log
            return;
        }
        catch (NotSupportedException ex)
        {
            // TODO: log
            return;
        }
        catch(Exception ex)
        {
            // TODO: log
            return;
        }

        _subscriptionChannel.BasicAck(args.DeliveryTag, multiple: false);
    }

    private async Task HandleEvent(string eventName, byte[] body)
    {
        if (!_events.TryGetValue(eventName, out var eventInfo))
        {
            // TODO: log
            return;
        }

        object @event = JsonSerializer.Deserialize(
            body, 
            eventInfo.EventType, 
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        using IServiceScope scope = _services.CreateScope();

        foreach (var handlerInfo in eventInfo.Handlers)
        {
            var handler = scope.ServiceProvider.GetRequiredService(handlerInfo.EventHandlerType);

            await (Task) handlerInfo.EventHandlerType
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
                _subscriptionChannel.BasicCancel(handlerInfo.Tag);
                eventInfo.Handlers.Remove(handlerInfo);
            }
        }

        if (eventInfo.Handlers.Count == 0)
        {
            _subscriptionChannel.QueueBind(queue: _settings.ClientName, exchange: BROKER_NAME, routingKey: eventName);
            _events.Remove(eventName);
            _logger.LogWarning("Event {Event} has no handlers", eventName);
        }
    }

    private string GetEventName<TEvent>() => typeof(TEvent).Name;

    private string GetEventName(IEvent @event) => @event.GetType().Name;

    public void Dispose()
    {
        _events.Clear();
        _subscriptionChannel.Dispose();
        _connection.Dispose();
    }
}

record SubscriptionInfo(Type EventType)
{
    public List<EventHandlerInfo> Handlers { get; } = new();
}

record EventHandlerInfo(string Tag, Type EventHandlerType);
