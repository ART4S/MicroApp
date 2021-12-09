using Basket.API.Model;

namespace Basket.API.Infrastructure.DataAccess;

public interface IBasketRepository
{
    Task<string[]> GetBuyers();
    Task<CustomerBasket?> Get(string buyerId);
    Task Update(CustomerBasket basket);
    Task Remove(string buyerId);
}
