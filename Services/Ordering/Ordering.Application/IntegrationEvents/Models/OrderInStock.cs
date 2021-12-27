namespace Ordering.Application.IntegrationEvents.Models;

public record OrderInStock(Guid OrderId, List<OrderItemInStock> Items);

public record OrderItemInStock(Guid ProductId, bool IsInStock);

