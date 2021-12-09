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
        CustomerBasket? basket = await _basketRepo.Get(@event.ItemId);

        if (basket is null)
        {
            // TODO: log
            return;
        }

        var items = basket.Items.Where(x => x.Id == @event.ItemId);

        foreach (BasketItem item in items)
        {
            if (item.Price != @event.NewPrice)
            {
                item.OldPrice = item.Price;
                item.Price = @event.NewPrice;
            }
        }

        await _basketRepo.Update(basket);
    }
}