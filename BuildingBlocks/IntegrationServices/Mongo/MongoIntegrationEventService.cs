using IntegrationServices.Common;
using IntegrationServices.Models;
using System.Reflection;

namespace IntegrationServices.Mongo;

public class MongoIntegrationEventService : IIntegrationEventService
{
    private readonly IMongoIntegrationDbContext _integrationDb;
    private readonly EventTypeProvider _eventTypes;

    public MongoIntegrationEventService(
        IMongoIntegrationDbContext integrationDb,
        Assembly integrationEventsAssembly)
    {
        _integrationDb = integrationDb;
        _eventTypes = new(integrationEventsAssembly);
    }

    public async Task<ICollection<IntegrationEvent>> GetPendingEvents()
    {
        var events = await _integrationDb.GetAll(
            filter: x => x.Status == DeliveryStatus.Pending, 
            orderBy: x => x.CreationDate);

        foreach (var @event in events)
            @event.DeserializeEvent(toType: _eventTypes.GetEventType(@event.EventType));

        return events.Select(x => x.IntegrationEvent).ToList();
    }

    public async Task MarkEventAsCompleted(Guid eventId)
    {
        var @event = await _integrationDb.GetById(eventId) ??
            throw new Exception($"Event with id={eventId} not found");

        @event.Complete();

        await _integrationDb.Update(@event);
        await _integrationDb.SaveChanges();
    }

    public async Task Publish(IntegrationEvent @event)
    {
        await _integrationDb.Add(new IntegrationEventLogEntry(@event));
        await _integrationDb.SaveChanges();
    }
}
