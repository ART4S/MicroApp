using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System.Net;
using Web.API.Settings;

namespace Web.API.Configuration.Factories;

public static class HttpPolicyFactory
{
    private static readonly PolicySettings _settings = new();

    public static void InitSettings(IConfiguration configuration)
    {
        configuration.GetSection("PolicySettings").Bind(_settings);
    }

    public static void AddDefaultPolicies(this IHttpClientBuilder clientBuilder)
    {
        clientBuilder
            .AddPolicyHandler(BuildTimeoutPolicy())
            .AddPolicyHandler(BuildRetryPolicy)
            .AddPolicyHandler(BuildCircuitBreakerPolicy());
    }

    private static IAsyncPolicy<HttpResponseMessage> BuildTimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(_settings.TimeoutSec);
    }

    private static IAsyncPolicy<HttpResponseMessage> BuildRetryPolicy(
        IServiceProvider services, HttpRequestMessage request)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount: _settings.RetryCount,
                attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                (response, _, attempt, _) =>
                {
                    services.GetRequiredService<ILoggerFactory>()
                        .CreateLogger("HttpPolicies")
                        .LogError("Got {StatusCode} from {Request} on attempt {Attempt}",
                            response.Result.StatusCode, 
                            response.Result.RequestMessage!.ToString(),
                            attempt);
                });
    }

    private static IAsyncPolicy<HttpResponseMessage> BuildCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .OrResult(x => x.StatusCode == HttpStatusCode.TooManyRequests)
            .CircuitBreakerAsync(
                _settings.CircuitBreaker.AttemptsBeforeBreak,
                TimeSpan.FromSeconds(_settings.CircuitBreaker.DurationOfBreakSec));
    }
}
