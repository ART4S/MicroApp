using EventBus.Abstractions;
using System.Text.Json.Serialization;

namespace IntegrationServices;

public abstract record IntegrationEvent : IEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreateDate = DateTime.Now;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime createDate)
    {
        Id = id;
        CreateDate = createDate;
    }

    public Guid Id { get; private init;  }

    public DateTime CreateDate { get; private init; }
}
