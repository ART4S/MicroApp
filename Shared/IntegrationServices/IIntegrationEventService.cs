namespace IntegrationServices;

public interface IIntegrationEventService
{
    Task<ICollection<IntegrationEvent>> GetPendingEvents();

    Task MarkEventAsCompleted(Guid eventId);

    Task Save(IntegrationEvent @event);
}
