using System.Text.Json;

namespace IntegrationServices.Models;

public class IntegrationEventLogEntry
{
    public IntegrationEventLogEntry() { }

    public IntegrationEventLogEntry(IntegrationEvent @event)
    {
        Id = @event.Id;
        CreationDate = @event.CreationDate;
        EventType = @event.GetType().FullName;
        Content = JsonSerializer.Serialize(@event, @event.GetType());
        Status = DeliveryStatus.Pending;
    }

    public Guid Id { get; private set; }
    public DateTime CreationDate { get; private set; }
    public string EventType { get; private set; }
    public string Content { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public IntegrationEvent IntegrationEvent { get; private set; }

    public void DeserializeEvent(Type toType)
    {
        IntegrationEvent = (IntegrationEvent)JsonSerializer.Deserialize(
            Content,
            toType,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }

    public void Complete() => Status = DeliveryStatus.Completed;
}
