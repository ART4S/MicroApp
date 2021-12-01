using IntegrationServices.Enums;
using System.Text.Json;

namespace IntegrationServices.Entities;

public class IntegrationEventLogEntry
{
    public IntegrationEventLogEntry() { }

    public IntegrationEventLogEntry(IntegrationEvent @event)
    {
        EventType = @event.GetType().FullName;
        Content = JsonSerializer.Serialize(@event, @event.GetType());
        Status = IntegrationEventStatus.Pending;
    }

    public Guid Id { get; private set; }
    public string EventType { get; private set; }
    public string Content { get; private set; }
    public IntegrationEventStatus Status { get; private set; }
    public IntegrationEvent IntegrationEvent { get; private set; }

    public void DeserializeEvent()
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, Type.GetType(EventType)) as IntegrationEvent;
    }

    public void Complete() => Status = IntegrationEventStatus.Completed;
}
