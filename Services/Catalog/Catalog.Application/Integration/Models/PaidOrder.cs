using System.Text.Json.Serialization;

namespace Catalog.Application.Integration.Models;

public record PaidOrder
{
    [JsonInclude]
    public Guid OrderId { get; private set; }

    [JsonInclude]
    public List<PaidOrderItem> Items { get; private set; }
}

public record PaidOrderItem
{
    [JsonInclude]
    public Guid ProductId { get; private set; }

    [JsonInclude]
    public int Quantity { get; private set; }
}
