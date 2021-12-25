namespace Ordering.Application.Models.Orders;

public class OrderEditDto
{
    public string? Description { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
    public Guid PaymentMethodId { get; set; }
}
