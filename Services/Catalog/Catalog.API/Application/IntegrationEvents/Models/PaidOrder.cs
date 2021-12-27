namespace Catalog.API.Application.IntegrationEvents.Models;

public record PaidOrder(Guid OrderId, Guid BuyerId, int OrderStatusId, List<PaidOrderItem> Items);

public record PaidOrderItem(Guid ProductId, int Quantity);
