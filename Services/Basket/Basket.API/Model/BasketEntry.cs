namespace Basket.API.Model
{
    public class BasketEntry
    {
        public BasketEntry(CustomerBasket basket)
        {
            Basket = basket;
        }

        public DateTime LastUpdate { get; set; }

        public CustomerBasket Basket { get; private set; }
    }
}
