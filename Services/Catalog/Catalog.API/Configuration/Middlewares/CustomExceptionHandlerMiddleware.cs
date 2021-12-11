using Catalog.Application.Exceptions;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Catalog.API.Configuration.Middlewares;

class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
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
            case CommandValidationException ex:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = MediaTypeNames.Application.Json;

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Message = ex.UserMessage,
                    Errors = ex.Errors
                }, new JsonSerializerOptions() 
                { 
                    WriteIndented = true, 
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull 
                });

                break;

            case InvalidRequestException ex:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsync(ex.UserMessage);
                break;

            case EntityNotFoundException ex:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsync(ex.UserMessage);
                break;

            default:
                var logger = httpContext.RequestServices
                    .GetRequiredService<ILogger<CustomExceptionHandlerMiddleware>>();

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
                Message = exception.Message,
                StackTrace = exception.StackTrace
            }, options));

            current = current.InnerException;

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
