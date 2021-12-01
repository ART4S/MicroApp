using Catalog.API.Configuration.Middlewares;

namespace Catalog.API.Configuration;

static class PipelineConfiguration
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
