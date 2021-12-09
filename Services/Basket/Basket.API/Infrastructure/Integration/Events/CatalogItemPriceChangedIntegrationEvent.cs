using IntegrationServices;

namespace Basket.API.Infrastructure.Integration.Events;

public record CatalogItemPriceChangedIntegrationEvent(string ItemId, decimal? NewPrice) : IntegrationEvent;
