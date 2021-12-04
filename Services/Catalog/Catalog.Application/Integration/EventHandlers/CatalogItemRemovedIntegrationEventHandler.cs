﻿using Catalog.Application.Integration.Events;
using Catalog.Application.Interfaces.DataAccess;
using EventBus.Abstractions;

namespace Catalog.Application.Integration.EventHandlers;

public class CatalogItemRemovedIntegrationEventHandler : IEventHandler<CatalogItemRemovedIntegrationEvent>
{
    private readonly IItemPictureRepository _pictureRepo;

    public CatalogItemRemovedIntegrationEventHandler(IItemPictureRepository pictureRepo)
    {
        _pictureRepo = pictureRepo;
    }

    public async Task Handle(CatalogItemRemovedIntegrationEvent @event)
    {
        if (@event.PictureName is not null)
            await _pictureRepo.RemovePictureByName(@event.PictureName);
    }
}