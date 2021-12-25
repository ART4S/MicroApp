using Basket.API.Infrastructure.DataAccess;
using Basket.API.Infrastructure.IntegrationEvents.Events;
using EventBus.Abstractions;

namespace Basket.API.Infrastructure.IntegrationEvents.EventHandlers;

public class OrderCreatedIntegrationEventHandler : IEventHandler<OrderCreatedIntegrationEvent>
{
    private readonly ILogger _logger;
    private readonly IBasketRepository _basketRepo;

    public OrderCreatedIntegrationEventHandler(
        ILogger<OrderCreatedIntegrationEventHandler> logger,
        IBasketRepository basketRepo)
    {
        _logger = logger;
        _basketRepo = basketRepo;
    }

    public async Task Handle(OrderCreatedIntegrationEvent @event)
    {
        await _basketRepo.Remove(@event.Order.BuyerId.ToString());

        // TODO: log
    }
}
