using Basket.API.Infrastructure.Integration.EventHandlers;
using Basket.API.Infrastructure.Integration.Events;
using EventBus.Abstractions;

namespace Basket.API.Configuration;

static class PipelineConfiguration
{
    public static void SubscribeToEvents(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<CatalogItemPriceChangedIntegrationEvent, CatalogItemPriceChangedIntegrationEventHandler>();
    }
}
