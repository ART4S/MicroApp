using System.Net.Mime;
using System.Text;

namespace Web.API.Exceptions;

public class InvalidRequestException : Exception
{
    public InvalidRequestException(int statusCode, Stream content, string contentType)
    {
        StatusCode = statusCode;
        Content = content;
        ContentType = contentType;
    }

    public int StatusCode { get; }
    public string ContentType { get; }
    public Stream Content { get; }

    public static InvalidRequestException BadRequest(string message)
    {
        MemoryStream ms = new();
        ms.Write(Encoding.UTF8.GetBytes(message));
        ms.Position = 0;
        return new InvalidRequestException(StatusCodes.Status400BadRequest, ms, MediaTypeNames.Text.Plain);
    }
}
