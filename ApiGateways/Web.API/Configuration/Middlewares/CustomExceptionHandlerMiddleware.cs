using Grpc.Core;
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
            case RpcException ex:
                httpContext.Response.StatusCode = HttpUtils.ConvertRpcStatusCodeToHttp(ex.StatusCode);

                if (!string.IsNullOrEmpty(ex.Message))
                {
                    Match math = Regex.Match(ex.Message, "Detail=\"(?'message'.+)\"");
                    if (math is not null)
                        await httpContext.Response.WriteAsync(math.Groups["message"].Value);
                }

                break;

            case InvalidRequestException ex:
                httpContext.Response.StatusCode = ex.StatusCode;
                httpContext.Response.ContentType = ex.ContentType;
                await ex.Content.CopyToAsync(httpContext.Response.Body);
                break;

            default:
                var logger = httpContext.RequestServices
                    .GetRequiredService<ILogger<CustomExceptionHandlerMiddleware>>();

                logger.LogError(exception, "Unknown type of exception '{ExceptionType}'", exception.GetType().Name);

                break;
        }
    }
}
