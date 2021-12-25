using Ordering.Application.Services;

namespace Ordering.Infrastructure.Common;

public class CurrentTime : ICurrentTime
{
    public DateTime Now => DateTime.UtcNow;
}
