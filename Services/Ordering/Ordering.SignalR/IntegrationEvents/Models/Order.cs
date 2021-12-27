namespace Ordering.SignalR.IntegrationEvents.Models;

public record Order(Guid OrderId, Guid BuyerId, int OrderStatusId);