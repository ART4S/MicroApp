using IntegrationServices;

namespace Catalog.Application.Integration.Events;

public record CatalogItemRemovedIntegrationEvent(Guid ItemId) : IntegrationEvent
{
}
