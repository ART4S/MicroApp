using System.Text.Json.Serialization;

namespace Basket.API.Models;

public class CustomerBasket
{
    [JsonConstructor]
    public CustomerBasket(List<BasketItem> items)
    {
        Items = items;
    }

    public CustomerBasket()
    {
        Items = new List<BasketItem>();
    }

    public string BuyerId { get; set; }

    public List<BasketItem> Items { get; }
}
