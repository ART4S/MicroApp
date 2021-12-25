using EventBus.Abstractions;
using HealthChecks.UI.Client;
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

    public static void MapHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/liveness", new()
        {
            Predicate = x => x.Name == "self"
        });

        endpoints.MapHealthChecks("/hc", new()
        {
            Predicate = x => x.Name != "self",
            AllowCachingResponses = false,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}