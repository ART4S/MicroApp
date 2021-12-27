using HealthChecks.UI.Client;
using Web.API.Configuration;
using Web.API.Configuration.Middlewares;

namespace Web.API.Configuration;

static class AppConfiguration
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }

    public static void MapHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/liveness", new()
        {
            Predicate = x => x.Name == "Self"
        });

        endpoints.MapHealthChecks("/hc", new()
        {
            Predicate = _ => true,
            AllowCachingResponses = false,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}
