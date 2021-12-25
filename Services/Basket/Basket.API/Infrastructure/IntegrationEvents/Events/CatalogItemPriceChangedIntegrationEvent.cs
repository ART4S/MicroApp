using IntegrationServices.Models;

namespace Basket.API.Infrastructure.IntegrationEvents.Events;

public record CatalogItemPriceChangedIntegrationEvent(string ItemId, decimal? NewPrice) : IntegrationEvent;
