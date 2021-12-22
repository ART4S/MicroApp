namespace Identity.API.Services;

public class CurrentTime : ICurrentTime
{
    public DateTime Now => DateTime.UtcNow;
}
