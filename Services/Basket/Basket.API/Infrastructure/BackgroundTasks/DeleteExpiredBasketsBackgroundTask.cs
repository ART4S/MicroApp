using Basket.API.Infrastructure.DataAccess;
using Basket.API.Infrastructure.Services;
using Basket.API.Model;
using TaskScheduling.Abstractions;

namespace Basket.API.Infrastructure.BackgroundTasks;

public class DeleteExpiredBasketsBackgroundTask : IBackgroundTask
{
    private readonly ICurrentTime _currentTime;
    private readonly IBasketRepository _basketRepo;
    private readonly TimeSpan _expiration;

    public DeleteExpiredBasketsBackgroundTask(
        ICurrentTime currentTime, 
        IBasketRepository basketRepo, 
        TimeSpan expiration)
    {
        _currentTime = currentTime;
        _basketRepo = basketRepo;
        _expiration = expiration;
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        string[] buyers = await _basketRepo.GetBuyers();

        List<string> expiredBaskets = new();
        
        DateTime now = _currentTime.Now;

        foreach(string buyerId in buyers)
        {
            BasketEntry? basket = await _basketRepo.Get(buyerId);

            if (basket is null)
            {
                // TODO: log
                continue;
            }

            if (now - basket.LastUpdate > _expiration)
                expiredBaskets.Add(buyerId);
        }

        foreach(string buyerId in expiredBaskets)
        {
            await _basketRepo.Remove(buyerId);
        }
    }
}
