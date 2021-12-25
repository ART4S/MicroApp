using System.Text.Json.Serialization;

namespace Ordering.Application.IntegrationEvents.Models;

public record OrderInStock
{
    [JsonInclude]
    public Guid OrderId { get; private set; }

    [JsonInclude]
    public List<OrderItemInStock> Items { get; private set; }
}

public record OrderItemInStock
{
    [JsonInclude]
    public Guid ProductId { get; private set; }

    [JsonInclude]
    public bool IsInStock { get; private set; }
}

