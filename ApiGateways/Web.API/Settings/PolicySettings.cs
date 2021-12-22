namespace Web.API.Settings;

public record PolicySettings
{
    public int RetryCount { get; set; }
    public int TimeoutSec { get; set; }
    public CircuitBreakerSettings CircuitBreaker { get; set; }
}

public record CircuitBreakerSettings
{
    public int AttemptsBeforeBreak { get; set; }
    public int DurationOfBreakSec { get; set; }
}
