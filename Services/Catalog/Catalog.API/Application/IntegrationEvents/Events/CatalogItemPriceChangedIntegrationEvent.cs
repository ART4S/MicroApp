using IntegrationServices.Models;

namespace Catalog.API.Application.IntegrationEvents.Events;

public record CatalogItemPriceChangedIntegrationEvent(Guid ItemId, decimal? NewPrice) : IntegrationEvent;
