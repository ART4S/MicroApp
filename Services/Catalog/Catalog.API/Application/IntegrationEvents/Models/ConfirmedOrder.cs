using System.Text.Json.Serialization;

namespace Catalog.API.Application.IntegrationEvents.Models;

public record ConfirmedOrder
{
    public ConfirmedOrder()
    {
        Items = new List<ConfirmedOrderItem>();
    }

    [JsonInclude]
    public Guid OrderId { get; private set; }

    [JsonInclude]
    public Guid BuyerId { get; set; }

    [JsonInclude]
    public int OrderStatusId { get; set; }

    [JsonInclude]
    public List<ConfirmedOrderItem> Items { get; private set; }
}

public record ConfirmedOrderItem
{
    [JsonInclude]
    public Guid ProductId { get; private set; }

    [JsonInclude]
    public int Quantity { get; private set; }
}

