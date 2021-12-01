namespace Catalog.Application.Exceptions;

public class NotFoundException : Exception
{
    public string UserMessage { get; }

    public NotFoundException(string itemName)
    {
        UserMessage = $"{itemName} not found";
    }
}
