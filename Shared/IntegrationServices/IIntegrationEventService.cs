namespace IntegrationServices;

public interface IIntegrationEventService
{
    Task Save(IntegrationEvent @event);
}
