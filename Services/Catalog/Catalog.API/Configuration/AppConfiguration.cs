using Catalog.API.Configuration.Middlewares;
using Catalog.Application.Integration.EventHandlers;
using Catalog.Application.Integration.Events;
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
    }
}
