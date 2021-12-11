using Basket.API.Infrastructure.DataAccess;
using Basket.API.Infrastructure.Integration.Events;
using Basket.API.Model;
using EventBus.Abstractions;

namespace Basket.API.Infrastructure.Integration.EventHandlers;

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
        BasketEntry? entry = await _basketRepo.Get(@event.ItemId);

        if (entry is null)
        {
            // TODO: log
            return;
        }

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