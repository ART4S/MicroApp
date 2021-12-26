using Basket.API.Models;

namespace Basket.API.Infrastructure.DataAccess;

public interface IBasketRepository
{
    Task<string[]> GetUsers();
    Task<BasketEntry?> Get(string userId);
    Task Update(BasketEntry basket);
    Task Remove(string userId);
}
