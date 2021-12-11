using EventBus.Abstractions;
using System.Text.Json.Serialization;

namespace IntegrationServices.Model;

public abstract record IntegrationEvent : IEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonInclude]
    public Guid Id { get; private init; }

    [JsonInclude]
    public DateTime CreationDate { get; private init; }
}
