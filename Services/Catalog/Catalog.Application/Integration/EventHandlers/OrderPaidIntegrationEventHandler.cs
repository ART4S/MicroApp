using Catalog.Application.Integration.Events;
using Catalog.Application.Services;
using EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Integration.EventHandlers;

public class OrderPaidIntegrationEventHandler : IEventHandler<OrderPaidIntegrationEvent>
{
    private readonly ILogger _logger;
    private readonly ICatalogDbContext _catalogDb;

    public OrderPaidIntegrationEventHandler(
        ILogger<OrderPaidIntegrationEventHandler> logger,
        ICatalogDbContext catalogDb)
    {
        _logger = logger;
        _catalogDb = catalogDb;
    }

    public async Task Handle(OrderPaidIntegrationEvent @event)
    {
        Guid[] ids = @event.Order.Items
            .Select(x => x.ProductId).ToArray();

        var items = await _catalogDb.CatalogItems
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id);

        foreach(var item in @event.Order.Items)
        {
            if (items.TryGetValue(item.ProductId, out var itemInDb))
            {
                if (itemInDb.AvailableInStock < item.Quantity)
                {
                    // TODO: log
                }

                itemInDb.AvailableInStock -= Math.Min(itemInDb.AvailableInStock, item.Quantity);
            }
            else
            {
                // TODO: log
            }
        }

        await _catalogDb.SaveChangesAsync();
    }
}