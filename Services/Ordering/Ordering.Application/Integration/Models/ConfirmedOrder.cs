namespace Ordering.Application.Integration.Models;

public record ConfirmedOrder
{
    public Guid OrderId { get; set; }
    public List<ConfirmedOrderItem> Items { get; set; } = new List<ConfirmedOrderItem>();
}

public record ConfirmedOrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
