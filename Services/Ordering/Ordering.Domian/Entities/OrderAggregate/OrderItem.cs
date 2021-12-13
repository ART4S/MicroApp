using Catalog.Domian.Abstractions;

namespace Ordering.Domian.Entities.OrderAggregate;

public class OrderItem : BaseEntity
{
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public bool InStock { get; set; }

    public Guid ProductId { get; set; }
}
