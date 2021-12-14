using EventBus.Abstractions;
using Payment.API.Integration.Events;

namespace Payment.API.Integration.EventHandlers;

public class OrderAcceptedIntegrationEventHandler : IEventHandler<OrderAcceptedIntegrationEvent>
{
    private readonly ILogger _logger;
    private readonly IEventBus _eventBus;

    public OrderAcceptedIntegrationEventHandler(
        ILogger<OrderAcceptedIntegrationEventHandler> logger,
        IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }

    public async Task Handle(OrderAcceptedIntegrationEvent @event)
    {
        try
        {
            IEvent integrationEvent;

            // Simulating payment process
            if (Random.Shared.Next() % 2 == 0)
                integrationEvent = new PaymentSucceedIntegrationEvent(@event.Order.OrderId);
            else
                integrationEvent = new PaymentFailedIntegrationEvent(@event.Order.OrderId);

            _eventBus.Publish(integrationEvent); // Not safe. Made for simplicity purposes
        }
        catch (Exception ex)
        {
            // TODO: log
            _logger.LogError("", ex);
        }
    }
}