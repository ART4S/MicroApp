namespace Ordering.Application.Integration.Models;

public record PaidOrder
{
    public Guid OrderId { get; set; }
    public List<PaidOrderItem> Items { get; set; } = new List<PaidOrderItem>();
}

public record PaidOrderItem
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
