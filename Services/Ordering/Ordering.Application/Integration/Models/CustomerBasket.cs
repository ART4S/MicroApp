using System.Text.Json.Serialization;

namespace Ordering.Application.Integration.Models;

public class CustomerBasket
{
    [JsonInclude]
    public Guid BuyerId { get; private set; }

    [JsonInclude]
    public List<BasketItem> Items { get; private set; }
}
