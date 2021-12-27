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
        string userId = @event.Order?.BuyerId.ToString();

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogError("UserId is missing");
            return;
        }

        _logger.LogInformation("Removing basket for user {UserId}", userId);

        try
        {
            await _basketRepo.Remove(userId);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error occured while removing basket fro user {UserId}", userId);
            return;
        }

        _logger.LogInformation("Removing basket for user {UserId} succeed", userId);
    }
}
