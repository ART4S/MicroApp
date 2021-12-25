namespace Ordering.Application.Models.Orders;

public class OrderItemDto
{
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public bool IsInStock { get; set; } = true;

    public Guid ProductId { get; set; }
}
