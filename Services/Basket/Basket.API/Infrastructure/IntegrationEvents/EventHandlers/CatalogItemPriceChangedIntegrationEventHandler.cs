using Basket.API.Infrastructure.DataAccess;
using Basket.API.Infrastructure.IntegrationEvents.Events;
using Basket.API.Models;
using EventBus.Abstractions;

namespace Basket.API.Infrastructure.IntegrationEvents.EventHandlers;

public class CatalogItemPriceChangedIntegrationEventHandler : IEventHandler<CatalogItemPriceChangedIntegrationEvent>
{
    private readonly ILogger _logger;
    private readonly IBasketRepository _basketRepo;

    public CatalogItemPriceChangedIntegrationEventHandler(
        ILogger<CatalogItemPriceChangedIntegrationEventHandler> logger,
        IBasketRepository basketRepo)
    {
        _logger = logger;
        _basketRepo = basketRepo;
    }

    public async Task Handle(CatalogItemPriceChangedIntegrationEvent @event)
    {
        string[] users = await _basketRepo.GetUsers();

        foreach (string userId in users)
        {
            BasketEntry? entry = await _basketRepo.Get(userId);

            if (entry is null) continue;

            var items = entry.Basket.Items.Where(x => x.ProductId == @event.ItemId);

            foreach (BasketItem item in items)
            {
                if (item.UnitPrice != @event.NewPrice)
                {
                    item.OldUnitPrice = item.UnitPrice;
                    item.UnitPrice = @event.NewPrice;
                }
            }

            await _basketRepo.Update(entry);
        }
    }
}