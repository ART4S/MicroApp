using IntegrationServices;

namespace Catalog.Application.Integration.Events;

public record CatalogItemPriceChangedIntegrationEvent(Guid ItemId, decimal? NewPrice) : IntegrationEvent;
