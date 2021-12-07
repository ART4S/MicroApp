namespace Basket.API.Model;

public class CustomerBasket
{
    public string BuyerId { get; set; }

    public List<BasketItem> Items { get; private set; } = new List<BasketItem>();

    public CustomerBasket(string buyerId)
    {
        BuyerId = buyerId;
    }
}
