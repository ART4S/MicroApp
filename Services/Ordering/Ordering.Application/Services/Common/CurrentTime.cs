namespace Ordering.Application.Services.Common;

public class CurrentTime : ICurrentTime
{
    public DateTime Now => DateTime.UtcNow;
}
