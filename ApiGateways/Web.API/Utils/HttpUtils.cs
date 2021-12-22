using Grpc.Core;
using System.Net;
using System.Net.Mime;
using Web.API.Exceptions;

namespace Web.API.Utils;

public static class HttpUtils
{
    public static async Task HandleErrorStatusCodes(HttpResponseMessage response)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
            case HttpStatusCode.Created:
            case HttpStatusCode.OK:
                return;

            default:
                throw new InvalidRequestException
                (
                    statusCode: (int)response.StatusCode,
                    content: await response.Content.ReadAsStreamAsync(),
                    contentType: response.Content.Headers.ContentType?.MediaType ?? MediaTypeNames.Application.Json
                );
        }
    }

    public static int ConvertRpcStatusCodeToHttp(StatusCode rpcStatusCode) =>
        rpcStatusCode switch
        {
            StatusCode.OK => StatusCodes.Status200OK,
            StatusCode.NotFound => StatusCodes.Status404NotFound,
            StatusCode.Unauthenticated => StatusCodes.Status401Unauthorized,
            StatusCode.PermissionDenied => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
}
