using Catalog.Domian.Abstractions;

namespace Ordering.Domian.Entities.OrderAggregate;

public class OrderItem : BaseEntity
{
    public Guid ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
}
