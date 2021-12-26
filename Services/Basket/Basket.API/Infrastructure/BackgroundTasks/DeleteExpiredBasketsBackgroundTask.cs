using Basket.API.Infrastructure.DataAccess;
using Basket.API.Infrastructure.Services;
using Basket.API.Models;
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
        DateTime now = _currentTime.Now;

        string[] users = await _basketRepo.GetUsers();

        foreach (string userId in users)
        {
            BasketEntry? basket = await _basketRepo.Get(userId);

            if (basket is not null && now - basket.LastUpdate > _expiration)
                await _basketRepo.Remove(userId);
        }
    }
}
