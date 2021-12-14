namespace Ordering.Application.Model.Orders;

public class OrderInfoDto
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public int StatusId { get; set; }

    public Guid? PaymentMethodId { get; set; }

    public string? Description { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Country { get; set; }

    public string ZipCode { get; set; }

    public ICollection<OrderItemDto> OrderItems { get; } = new HashSet<OrderItemDto>();
}
