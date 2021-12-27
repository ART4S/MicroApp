using Grpc.Core;
using Polly.CircuitBreaker;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Web.API.Exceptions;
using Web.API.Utils;

namespace Web.API.Configuration.Middlewares;

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
            case BrokenCircuitException ex:
                {
                    var logger = httpContext.RequestServices
                        .GetRequiredService<ILogger<CustomExceptionHandlerMiddleware>>();

                    logger.LogError(ex, "Internal error occured in service");

                    httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;

                    await httpContext.Response.WriteAsync("Service temporary unavaliable");
                    break;
                }

            case RpcException ex:
                {
                    httpContext.Response.StatusCode = HttpUtils.ConvertRpcStatusCodeToHttp(ex.StatusCode);

                    if (!string.IsNullOrEmpty(ex.Message))
                    {
                        Match math = Regex.Match(ex.Message, "Detail=\"(?'message'.+)\"");
                        if (math is not null)
                            await httpContext.Response.WriteAsync(math.Groups["message"].Value);
                    }

                    break;
                }

            case InvalidRequestException ex:
                {
                    httpContext.Response.StatusCode = ex.StatusCode;
                    httpContext.Response.ContentType = ex.ContentType;
                    await ex.Content.CopyToAsync(httpContext.Response.Body);
                    break;
                }

            case InvalidOperationException ex when ex.Message.Contains("IDX20803: Unable to obtain configuration"):
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("Invalid token");
                    break;
                }

            default:
                {
                    var logger = httpContext.RequestServices
                        .GetRequiredService<ILogger<CustomExceptionHandlerMiddleware>>();

                    logger.LogError(exception, "Unknown type of exception '{ExceptionType}'", exception.GetType().Name);

                    break;
                }
        }
    }
}
