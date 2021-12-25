using Catalog.API.Application.IntegrationEvents.Events;
using Catalog.API.DataAccess.Repositories;
using EventBus.Abstractions;

namespace Catalog.API.Application.IntegrationEvents.EventHandlers;

public class CatalogItemRemovedIntegrationEventHandler : IEventHandler<CatalogItemRemovedIntegrationEvent>
{
    private readonly IPictureRepository _pictureRepo;

    public CatalogItemRemovedIntegrationEventHandler(IPictureRepository pictureRepo)
    {
        _pictureRepo = pictureRepo;
    }

    public async Task Handle(CatalogItemRemovedIntegrationEvent @event)
    {
        if (@event.PictureName is not null)
            await _pictureRepo.RemovePicture(@event.PictureName);
    }
}