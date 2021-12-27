namespace Basket.API.Infrastructure.IntegrationEvents.Models;

public record CreatedOrder(Guid OrderId, Guid BuyerId, int OrderStatusId);