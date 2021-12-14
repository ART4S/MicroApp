using Catalog.Domian.Abstractions;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domian.Dictionaries;

namespace Ordering.Domian.Entities;

public class Order : BaseEntity
{
    public Order()
    {
        OrderItems = new HashSet<OrderItem>();
    }

    public DateTime OrderDate { get; set; }

    public int OrderStatusId { get; set; }
    public OrderStatusDict OrderStatus { get; set; }

    public string? Description { get; set; }

    public Address Address { get; set; }

    public Guid BuyerId { get; set; }
    public Buyer Buyer { get; set; }

    public Guid? PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public ICollection<OrderItem> OrderItems { get; private set; }
}