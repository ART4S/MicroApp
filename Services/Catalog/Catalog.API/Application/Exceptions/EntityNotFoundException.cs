namespace Catalog.API.Application.Exceptions;

public class EntityNotFoundException : Exception
{
    public string UserMessage { get; }

    public EntityNotFoundException(string itemName)
    {
        UserMessage = $"{itemName} not found";
    }
}
