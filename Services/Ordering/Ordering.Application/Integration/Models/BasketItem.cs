namespace Ordering.Application.Integration.Models;

public class BasketItem
{
    public Guid ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
