using IntegrationServices.Models;

namespace Catalog.API.Application.IntegrationEvents.Events;

public record CatalogItemRemovedIntegrationEvent(Guid ItemId, string? PictureName) : IntegrationEvent;
