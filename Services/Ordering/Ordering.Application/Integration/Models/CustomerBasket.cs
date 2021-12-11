using System.Text.Json.Serialization;

namespace Ordering.Application.Integration.Models;

public class CustomerBasket
{
    public CustomerBasket(List<BasketItem> items)
    {
        Items = items;
    }

    public Guid BuyerId { get; set; }

    public List<BasketItem> Items { get; private init; }
}
