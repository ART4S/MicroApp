namespace Basket.API.Model;

public class CustomerBasket
{
    public string BuyerId { get; set; }

    public DateTime LastUpdate { get; set; }

    public List<BasketItem> Items { get; private set; } = new List<BasketItem>();
}
