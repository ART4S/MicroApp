using Catalog.Application.Integration.Events;
using Catalog.Application.Integration.Model;
using Catalog.Application.Services.DataAccess;
using EventBus.Abstractions;
using IntegrationServices;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Integration.EventHandlers;

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

        Dictionary<Guid, int> itemsQuantityInDb = await _catalogDb.CatalogItems
            .Where(x => productIds.Contains(x.Id))
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Id, x => x.AvailableInStock);

        OrderInStock checkedOrder = new() { OrderId = @event.Order.OrderId };

        foreach(var item in @event.Order.Items)
        {
            bool isInStock = false;
            if (itemsQuantityInDb.TryGetValue(item.ProductId, out int availableInStock))
                isInStock = item.Quantity <= availableInStock;

            checkedOrder.Items.Add(new() { ProductId = item.ProductId, IsInStock = isInStock });
        }

        await _integrationEvents.Save(new OrderInStockCheckedIntegrationEvent(checkedOrder));
    }
}