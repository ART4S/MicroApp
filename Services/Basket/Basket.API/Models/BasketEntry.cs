namespace Basket.API.Models;

public class BasketEntry
{
    public BasketEntry(CustomerBasket basket)
    {
        Basket = basket;
    }

    public string UserId { get; set; }

    public DateTime LastUpdate { get; set; }

    public CustomerBasket Basket { get; private set; }
}
