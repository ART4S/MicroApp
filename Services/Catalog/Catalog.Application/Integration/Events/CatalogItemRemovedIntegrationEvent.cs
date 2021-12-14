using IntegrationServices.Models;

namespace Catalog.Application.Integration.Events;

public record CatalogItemRemovedIntegrationEvent(Guid ItemId, string? PictureName) : IntegrationEvent;
