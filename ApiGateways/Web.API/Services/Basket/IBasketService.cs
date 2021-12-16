using Web.API.Models.Basket;

namespace Web.API.Services.Basket;

public interface IBasketService
{
    Task<BasketDto> GetBasket();
    Task<BasketDto> UpdateBasket(BasketDto basket);
    Task CheckoutBasket(Guid requestId);
}
