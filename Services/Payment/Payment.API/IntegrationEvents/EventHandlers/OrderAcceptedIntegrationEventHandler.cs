using EventBus.Abstractions;
using Payment.API.IntegrationEvents.Events;

namespace Payment.API.IntegrationEvents.EventHandlers;

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
        _logger.LogInformation("Start processing event {@Event}", @event);

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
            _logger.LogError(ex, "Error occured while processing event {@Event}", @event.Id);
            return;
        }

        _logger.LogInformation("Processing event {@Event} succeed", @event.Id);
    }
}