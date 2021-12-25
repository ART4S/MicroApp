using System.Text.Json.Serialization;

namespace Catalog.API.Application.IntegrationEvents.Models;

public record PaidOrder
{
    [JsonInclude]
    public Guid OrderId { get; private set; }

    [JsonInclude]
    public Guid BuyerId { get; set; }

    [JsonInclude]
    public int OrderStatusId { get; set; }

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
