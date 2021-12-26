namespace Ordering.Application.IntegrationEvents.Models;

public record CustomerBasket(List<BasketItem> Items);

public record BasketItem(Guid ProductId, decimal UnitPrice, int Quantity);

