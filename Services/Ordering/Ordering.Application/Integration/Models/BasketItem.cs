using System.Text.Json.Serialization;

namespace Ordering.Application.Integration.Models;

public class BasketItem
{
    [JsonInclude]
    public Guid ProductId { get; private set; }

    [JsonInclude]
    public decimal UnitPrice { get; private set; }

    [JsonInclude]
    public int Quantity { get; private set; }
}
