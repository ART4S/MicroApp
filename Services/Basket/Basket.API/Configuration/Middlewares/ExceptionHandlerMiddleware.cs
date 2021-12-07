using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Basket.API.Configuration.Middlewares;

class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(ex, context);
        }
    }

    private static async Task HandleException(Exception exception, HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = MediaTypeNames.Text.Plain;

        switch (exception)
        {
            default:
                var logger = httpContext.RequestServices
                    .GetRequiredService<ILogger<ExceptionHandlerMiddleware>>();

                logger.LogError(exception, "Unknown type of exception '{ExceptionType}'", exception.GetType().Name);

                if (exception is not null)
                {
                    var env = httpContext.RequestServices.GetService<IWebHostEnvironment>();
                    if (env.IsDevelopment())
                        await httpContext.Response.WriteAsync(BuildDeveloperExceptionMessage(exception));
                }

                break;
        }
    }

    private static string BuildDeveloperExceptionMessage(Exception exception)
    {
        StringBuilder sb = new();

        Exception? current = exception;

        JsonSerializerOptions options = new() { WriteIndented = true };

        while (current is not null)
        {
            sb.AppendLine(JsonSerializer.Serialize(new
            {
                Type = exception.GetType().Name,
                exception.Message,
                exception.StackTrace
            }, options));

            current = current.InnerException;

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
