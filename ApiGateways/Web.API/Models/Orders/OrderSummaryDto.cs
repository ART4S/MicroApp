namespace Web.API.Models.Orders;

public class OrderSummaryDto
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public int StatusId { get; set; }

    public decimal Total { get; set; }
}
