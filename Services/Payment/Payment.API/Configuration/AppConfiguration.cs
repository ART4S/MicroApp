using EventBus.Abstractions;
using Payment.API.Integration.EventHandlers;
using Payment.API.Integration.Events;

namespace Payment.API.Configuration;

static class AppConfiguration
{
    public static void SubscribeToEvents(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<OrderAcceptedIntegrationEvent, OrderAcceptedIntegrationEventHandler>();
    }
}