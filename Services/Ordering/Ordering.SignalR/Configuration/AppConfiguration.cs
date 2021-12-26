using EventBus.Abstractions;
using Ordering.SignalR.IntegrationEvents.EventHandlers;
using Ordering.SignalR.IntegrationEvents.Events;

namespace Ordering.SignalR.Configuration;

static class AppConfiguration
{
    public static void SubscribeToEvents(this IApplicationBuilder app)
    {
        IEventBus eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
        eventBus.Subscribe<OrderConfirmedIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
        eventBus.Subscribe<OrderAcceptedIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
        eventBus.Subscribe<OrderPaidIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
        eventBus.Subscribe<OrderCancelledIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
    }
}
