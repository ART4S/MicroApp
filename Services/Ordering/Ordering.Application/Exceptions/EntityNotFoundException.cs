namespace Ordering.Application.Exceptions;

public class EntityNotFoundException : Exception
{
    public string UserMessage { get; }

    public EntityNotFoundException(string entityName)
    {
        UserMessage = $"{entityName} not found";
    }
}
