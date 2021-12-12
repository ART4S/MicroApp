using Ordering.Application.Services.Common;

namespace Ordering.Infrastructure.Common;

public class CurrentTime : ICurrentTime
{
    public DateTime Now => DateTime.UtcNow;
}
