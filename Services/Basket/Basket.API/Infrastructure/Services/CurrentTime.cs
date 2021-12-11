namespace Basket.API.Infrastructure.Services;

public class CurrentTime : ICurrentTime
{
    public DateTime Now => DateTime.UtcNow;
}
