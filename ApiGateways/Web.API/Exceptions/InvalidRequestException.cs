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
}
