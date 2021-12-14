namespace Ordering.Application.Model.Orders;

public class OrderItemDto
{
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public bool IsInStock { get; set; }

    public Guid ProductId { get; set; }
}
