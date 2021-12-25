using Catalog.API.Application.IntegrationEvents.EventHandlers;
using Catalog.API.Application.IntegrationEvents.Events;
using Catalog.API.Configuration.Middlewares;
using EventBus.Abstractions;

namespace Catalog.API.Configuration;

static class AppConfiguration
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }

    public static void SubscribeToEvents(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<CatalogItemRemovedIntegrationEvent, CatalogItemRemovedIntegrationEventHandler>();
        eventBus.Subscribe<OrderConfirmedIntegrationEvent, OrderConfirmedIntegrationEventHandler>();
        eventBus.Subscribe<OrderPaidIntegrationEvent, OrderPaidIntegrationEventHandler>();
    }
}
