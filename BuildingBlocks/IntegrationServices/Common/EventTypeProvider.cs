using IntegrationServices.Models;
using System.Reflection;

namespace IntegrationServices.Common;

internal class EventTypeProvider
{
    private readonly Dictionary<string, Type> _eventTypes;

    public EventTypeProvider(Assembly assembly)
    {
        _eventTypes = assembly
            .GetExportedTypes()
            .Where(x => x.IsSubclassOf(typeof(IntegrationEvent)))
            .ToDictionary(x => x.FullName);

        if (_eventTypes.Count == 0)
            throw new Exception($"There are no event types in assembly '{assembly.FullName}'");
    }

    public Type GetEventType(string eventTypeName)
    {
        if (_eventTypes.TryGetValue(eventTypeName, out var eventType))
            return eventType;
        else
            throw new Exception($"Type '{eventTypeName}' is missing");
    }
}
