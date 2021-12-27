using Catalog.API.Application.IntegrationEvents.Events;
using Catalog.API.DataAccess;
using Catalog.API.Models;
using EventBus.Abstractions;

namespace Catalog.API.Application.IntegrationEvents.EventHandlers;

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

        Dictionary<Guid, CatalogItem> items =
            (await _catalogDb.Products.GetByIds(ids)).ToDictionary(x => x.Id);

        foreach (var item in @event.Order.Items)
        {
            if (items.TryGetValue(item.ProductId, out var itemInDb))
            {
                if (itemInDb.AvailableInStock < item.Quantity)
                {
                    _logger.LogWarning("AvailableInStock cannot be less than requested Quantity");
                }

                itemInDb.AvailableInStock -= Math.Min(itemInDb.AvailableInStock, item.Quantity);
            }
            else
            {
                _logger.LogError("Product {ProductId} not found", item.ProductId);
            }
        }

        await _catalogDb.SaveChanges();
    }
}