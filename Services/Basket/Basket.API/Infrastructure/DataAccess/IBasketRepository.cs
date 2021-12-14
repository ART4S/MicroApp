using Basket.API.Models;

namespace Basket.API.Infrastructure.DataAccess;

public interface IBasketRepository
{
    Task<string[]> GetBuyers();
    Task<BasketEntry?> Get(string buyerId);
    Task Update(BasketEntry basket);
    Task Remove(string buyerId);
}
