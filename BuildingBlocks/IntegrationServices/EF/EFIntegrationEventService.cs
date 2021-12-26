using IntegrationServices.Models;
using IntegrationServices.Utils;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IntegrationServices.EF;

public class EFIntegrationEventService : IIntegrationEventService
{
    private readonly IEFIntegrationDbContext _integrationDb;
    private readonly EventTypeProvider _eventTypes;

    public EFIntegrationEventService(IEFIntegrationDbContext integrationDb, Assembly integrationEventsAssembly)
    {
        _integrationDb = integrationDb;
        _eventTypes = new(integrationEventsAssembly);
    }

    public async Task<ICollection<IntegrationEvent>> GetPendingEvents()
    {
        var events = await _integrationDb.IntegrationEvents
            .Where(x => x.Status == DeliveryStatus.Pending)
            .OrderBy(x => x.CreationDate)
            .ToListAsync();

        events.ForEach(x => x.DeserializeEvent(toType: _eventTypes.GetEventType(x.EventType)));

        return events.Select(x => x.IntegrationEvent).ToList();
    }

    public async Task MarkEventAsCompleted(Guid eventId)
    {
        var @event = await _integrationDb.IntegrationEvents.FindAsync(eventId) ??
            throw new Exception($"Event with id={eventId} not found");

        @event.Complete();

        await _integrationDb.SaveChangesAsync();
    }

    public async Task Publish(IntegrationEvent @event)
    {
        await _integrationDb.IntegrationEvents.AddAsync(new IntegrationEventLogEntry(@event));
        await _integrationDb.SaveChangesAsync();
    }
}