using Catalog.Application.Integration.Events;
using Catalog.Application.Services;
using EventBus.Abstractions;

namespace Catalog.Application.Integration.EventHandlers;

public class CatalogItemRemovedIntegrationEventHandler : IEventHandler<CatalogItemRemovedIntegrationEvent>
{
    private readonly IPicturesRepository _pictureRepo;

    public CatalogItemRemovedIntegrationEventHandler(IPicturesRepository pictureRepo)
    {
        _pictureRepo = pictureRepo;
    }

    public async Task Handle(CatalogItemRemovedIntegrationEvent @event)
    {
        if (@event.PictureName is not null)
            await _pictureRepo.RemovePicture(@event.PictureName);
    }
}