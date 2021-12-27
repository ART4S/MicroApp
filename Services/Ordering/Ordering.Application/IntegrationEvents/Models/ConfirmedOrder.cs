namespace Ordering.Application.IntegrationEvents.Models;

public record ConfirmedOrder
{
    public Guid OrderId { get; set; }
    public Guid BuyerId { get; set; }
    public int OrderStatusId { get; set; }
    public List<ConfirmedOrderItem> Items { get; set; }
}

public record ConfirmedOrderItem(Guid ProductId, int Quantity);
