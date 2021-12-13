namespace Ordering.Application.Integration.Models;

public record ConfirmedOrder
{
    public Guid OrderId { get; set; }
    public List<ConfirmedOrderItem> Items { get; set; }
}
