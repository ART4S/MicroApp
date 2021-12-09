using Basket.API.Infrastructure.Services;
using Basket.API.Model;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.API.Infrastructure.DataAccess;

class BasketRepository : IBasketRepository
{
    private readonly ILogger _logger;
    private readonly ICurrentTimeService _currentTime;
    private readonly IConnectionMultiplexer _connection;
    private readonly IDatabase _database;

    public BasketRepository(
        ILogger<BasketRepository> logger, 
        ICurrentTimeService currentTime,
        IConnectionMultiplexer connection)
    {
        _logger = logger;
        _currentTime = currentTime;
        _connection = connection;
        _database = connection.GetDatabase();
    }

    public async Task<string[]> GetBuyers()
    {
        return GetServer().Keys().Select(x => x.ToString()).ToArray();
    }

    public async Task<CustomerBasket?> Get(string buyerId)
    {
        var basket = await _database.StringGetAsync(buyerId);

        if (basket.IsNull)
            return null;

        return JsonSerializer.Deserialize<CustomerBasket>(
            basket,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = false });
    }

    public async Task Update(CustomerBasket basket)
    {
        basket.LastUpdate = _currentTime.Now;

        await _database.StringSetAsync(basket.BuyerId, JsonSerializer.Serialize(basket));
    }

    public async Task Remove(string buyerId)
    {
        await _database.KeyDeleteAsync(buyerId);
    }

    private IServer GetServer()
    {
        var endpoint = _connection.GetEndPoints().First();
        var server = _connection.GetServer(endpoint);

        return server;
    }
}
