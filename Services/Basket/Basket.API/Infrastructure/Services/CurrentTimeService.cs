namespace Basket.API.Infrastructure.Services;

public class CurrentTimeService : ICurrentTimeService
{
    public DateTime Now => DateTime.UtcNow;
}
