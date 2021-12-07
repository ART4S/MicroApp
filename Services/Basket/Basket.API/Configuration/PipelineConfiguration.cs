using Basket.API.Configuration.Middlewares;
using EventBus.Abstractions;

namespace Basket.API.Configuration;

static class PipelineConfiguration
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }

    public static void SubscribeToEvents(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    }
}
