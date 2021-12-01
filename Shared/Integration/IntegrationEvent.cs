using EventBus.Abstractions;

namespace Integration;

public abstract class IntegrationEvent : IEvent
{
    public IntegrationEvent() : this(Guid.NewGuid(), DateTime.Now)
    {
    }

    public IntegrationEvent(Guid id, DateTime createDate)
    {
        (Id, CreateDate) = (id, createDate);
    }

    public Guid Id { get; }

    public DateTime CreateDate { get; }
}
