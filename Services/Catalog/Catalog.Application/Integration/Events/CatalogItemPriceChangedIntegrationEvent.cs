using IntegrationServices;

namespace Catalog.Application.Integration.Events;

public record CatalogItemPriceChangedIntegrationEvent(decimal? NewPrice) : IntegrationEvent;
