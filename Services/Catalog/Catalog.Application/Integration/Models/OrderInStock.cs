namespace Catalog.Application.Integration.Models;

public record OrderInStock
{
    public Guid OrderId { get; set; }

    public List<OrderItemInStock> Items { get; set; } = new List<OrderItemInStock>();
}

public record OrderItemInStock
{
    public Guid ProductId { get; set; }

    public bool IsInStock { get; set; }
}

