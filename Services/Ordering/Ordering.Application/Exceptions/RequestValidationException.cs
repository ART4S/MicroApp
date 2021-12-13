using FluentValidation.Results;

namespace Ordering.Application.Exceptions;

public class RequestValidationException : Exception
{
    public string UserMessage { get; }

    public Dictionary<string, string[]> Errors { get; }

    public RequestValidationException(ValidationFailure[] failures)
    {
        Errors = failures
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToArray());
    }

    public RequestValidationException(string message)
    {
        UserMessage = message;
    }
}
