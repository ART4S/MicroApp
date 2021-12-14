using System.Text.Json.Serialization;

namespace Ordering.Application.Integration.Models;

public record CustomerBasket
{
    [JsonInclude]
    public Guid BuyerId { get; private set; }

    [JsonInclude]
    public List<BasketItem> Items { get; private set; }
}

public record BasketItem
{
    [JsonInclude]
    public Guid ProductId { get; private set; }

    [JsonInclude]
    public decimal UnitPrice { get; private set; }

    [JsonInclude]
    public int Quantity { get; private set; }
}

