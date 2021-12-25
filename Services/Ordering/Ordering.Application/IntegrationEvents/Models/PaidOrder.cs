namespace Ordering.Application.IntegrationEvents.Models;

public record PaidOrder
{
    public Guid OrderId { get; set; }
    public Guid BuyerId { get; set; }
    public int OrderStatusId { get; set; }
    public List<PaidOrderItem> Items { get; set; } = new List<PaidOrderItem>();
}

public record PaidOrderItem
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
