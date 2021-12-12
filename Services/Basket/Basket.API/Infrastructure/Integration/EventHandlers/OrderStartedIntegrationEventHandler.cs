using Basket.API.Infrastructure.DataAccess;
using Basket.API.Infrastructure.Integration.Events;
using EventBus.Abstractions;

namespace Basket.API.Infrastructure.Integration.EventHandlers;

public class OrderStartedIntegrationEventHandler : IEventHandler<OrderStartedIntegrationEvent>
{
    private readonly ILogger _logger;
    private readonly IBasketRepository _basketRepo;

    public OrderStartedIntegrationEventHandler(
        ILogger<OrderStartedIntegrationEventHandler> logger,
        IBasketRepository basketRepo)
    {
        _logger = logger;
        _basketRepo = basketRepo;
    }

    public async Task Handle(OrderStartedIntegrationEvent @event)
    {
        await _basketRepo.Remove(@event.BuyerId.ToString());

        // TODO: log
    }
}
