using Catalog.API.Application.IntegrationEvents.EventHandlers;
using Catalog.API.Application.IntegrationEvents.Events;
using Catalog.API.Configuration.Middlewares;
using EventBus.Abstractions;
using HealthChecks.UI.Client;

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

    public static void MapHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/liveness", new()
        {
            Predicate = x => x.Name == "Self"
        });

        endpoints.MapHealthChecks("/hc", new()
        {
            Predicate = _ => true,
            AllowCachingResponses = false,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}
