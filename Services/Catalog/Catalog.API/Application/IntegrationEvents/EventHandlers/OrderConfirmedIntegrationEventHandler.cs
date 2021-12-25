using Catalog.API.Application.IntegrationEvents.Events;
using Catalog.API.Application.IntegrationEvents.Models;
using Catalog.API.DataAccess;
using EventBus.Abstractions;
using IntegrationServices;

namespace Catalog.API.Application.IntegrationEvents.EventHandlers;

public class OrderConfirmedIntegrationEventHandler : IEventHandler<OrderConfirmedIntegrationEvent>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IIntegrationEventService _integrationEvents;

    public OrderConfirmedIntegrationEventHandler(
        ICatalogDbContext catalogDb,
        IIntegrationEventService integrationEvents)
    {
        _catalogDb = catalogDb;
        _integrationEvents = integrationEvents;
    }

    public async Task Handle(OrderConfirmedIntegrationEvent @event)
    {
        Guid[] productIds = @event.Order.Items.Select(x => x.ProductId).ToArray();

        Dictionary<Guid, int> productsQuantityInDb = (await _catalogDb.Products.GetByIds(productIds))
            .ToDictionary(x => x.Id, x => x.AvailableInStock);

        OrderInStock checkedOrder = new() { OrderId = @event.Order.OrderId };

        foreach (var item in @event.Order.Items)
        {
            bool isInStock = false;
            if (productsQuantityInDb.TryGetValue(item.ProductId, out int availableInStock))
                isInStock = item.Quantity <= availableInStock;

            checkedOrder.Items.Add(new() { ProductId = item.ProductId, IsInStock = isInStock });
        }

        await _integrationEvents.Publish(new OrderInStockCheckedIntegrationEvent(checkedOrder));
    }
}