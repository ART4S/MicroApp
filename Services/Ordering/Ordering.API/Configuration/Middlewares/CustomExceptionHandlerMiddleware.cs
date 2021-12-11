using Ordering.Application.Exceptions;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ordering.API.Configuration.Middlewares;

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

            //case InvalidRequestException ex:
            //    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            //    await httpContext.Response.WriteAsync(ex.UserMessage);
            //    break;

            //case EntityNotFoundException ex:
            //    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            //    await httpContext.Response.WriteAsync(ex.UserMessage);
            //    break;

            default:
                var logger = httpContext.RequestServices
                    .GetRequiredService<ILogger<CustomExceptionHandlerMiddleware>>();

                logger.LogError(exception, "Unknown type of exception '{ExceptionType}'", exception.GetType().Name);

                break;
        }
    }
}
