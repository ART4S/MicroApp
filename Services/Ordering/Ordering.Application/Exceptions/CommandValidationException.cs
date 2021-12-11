using FluentValidation.Results;

namespace Ordering.Application.Exceptions;

public class CommandValidationException : Exception
{
    public string UserMessage { get; }

    public Dictionary<string, string[]> Errors { get; }

    public CommandValidationException(ValidationFailure[] failures)
    {
        Errors = failures
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToArray());
    }

    public CommandValidationException(string message)
    {
        UserMessage = message;
    }
}
