namespace Catalog.API.Application.IntegrationEvents.Models;

public record ConfirmedOrder(Guid OrderId, Guid BuyerId, int OrderStatusId, List<ConfirmedOrderItem> Items);

public record ConfirmedOrderItem(Guid ProductId, int Quantity);
