using Basket.API.Model;

namespace Basket.API.Infrastructure.DataAccess;

public interface IBasketRepository
{
    Task<CustomerBasket?> Get(string buyerId);
    Task AddOrUpdate(CustomerBasket basket);
}
