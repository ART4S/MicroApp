namespace Ordering.Application.Exceptions;

public class InvalidRequestException : Exception
{
    public string UserMessage { get; }

    public InvalidRequestException(string message)
    {
        UserMessage = message;
    }
}