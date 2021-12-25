using EventBus.Abstractions;
using Payment.API.IntegrationEvents.EventHandlers;
using Payment.API.IntegrationEvents.Events;

namespace Payment.API.Configuration;

static class AppConfiguration
{
    public static void SubscribeToEvents(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<OrderAcceptedIntegrationEvent, OrderAcceptedIntegrationEventHandler>();
    }
}