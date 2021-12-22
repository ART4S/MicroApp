using Catalog.Domian.Abstractions;

namespace Ordering.Domian.Entities;

public class OrderItem : BaseEntity
{
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public bool IsInStock { get; set; } = true;

    public Guid ProductId { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}
