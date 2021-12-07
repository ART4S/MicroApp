using AutoMapper;
using Basket.API.Model;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.API.Infrastructure.DataAccess;

class BasketRepository : IBasketRepository
{
    private readonly ILogger _logger;
    private readonly IConnectionMultiplexer _connection;
    private readonly IDatabase _database;
    private readonly IMapper _mapper;

    public BasketRepository(
        ILogger<BasketRepository> logger, 
        IConnectionMultiplexer connection, 
        IMapper mapper)
    {
        _logger = logger;
        _connection = connection;
        _database = connection.GetDatabase();
        _mapper = mapper;
    }

    public async Task<CustomerBasket?> Get(string buyerId)
    {
        var basket = await _database.StringGetAsync(buyerId);

        if (basket.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<CustomerBasket>(
            basket,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = false });
    }

    public async Task AddOrUpdate(CustomerBasket basket)
    {
        CustomerBasket? existingBasket = await Get(basket.BuyerId);

        if (existingBasket is not null)
            _mapper.Map(basket, existingBasket);

        basket = existingBasket ?? basket;

        await _database.StringSetAsync(basket.BuyerId, JsonSerializer.Serialize(basket));
    }
}
