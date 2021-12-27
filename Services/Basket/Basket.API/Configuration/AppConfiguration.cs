using Basket.API.Infrastructure.IntegrationEvents.EventHandlers;
using Basket.API.Infrastructure.IntegrationEvents.Events;
using EventBus.Abstractions;
using HealthChecks.UI.Client;

namespace Basket.API.Configuration;

static class AppConfiguration
{
    public static void SubscribeToEvents(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<CatalogItemPriceChangedIntegrationEvent, CatalogItemPriceChangedIntegrationEventHandler>();
        eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
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
