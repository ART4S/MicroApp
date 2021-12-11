using IntegrationServices.DataAccess;
using IntegrationServices.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IntegrationServices;

public class IntegrationEventService : IIntegrationEventService
{
    private readonly IIntegrationDbContext _integrationDb;
    private readonly Dictionary<string, Type> _integrationEventTypes;

    public IntegrationEventService(IIntegrationDbContext integrationDb, Assembly integrationEventsAssembly)
    {
        _integrationDb = integrationDb;
        _integrationEventTypes = integrationEventsAssembly
            .GetExportedTypes()
            .Where(x => x.IsSubclassOf(typeof(IntegrationEvent)))
            .ToDictionary(x => x.FullName);
    }

    public async Task<ICollection<IntegrationEvent>> GetPendingEvents()
    {
        var events = await _integrationDb.IntegrationEvents
            .Where(x => x.Status == DeliveryStatus.Pending)
            .OrderBy(x => x.CreationDate)
            .ToListAsync();

        events.ForEach(x => x.DeserializeEvent(toType: _integrationEventTypes[x.EventType]));

        return events.Select(x => x.IntegrationEvent).ToList();
    }

    public async Task MarkEventAsCompleted(Guid eventId)
    {
        var @event = await _integrationDb.IntegrationEvents.FindAsync(eventId) ?? 
            throw new Exception($"Event with id={eventId} not found");

        @event.Complete();

        await _integrationDb.SaveChangesAsync();
    }

    public async Task Save(IntegrationEvent @event)
    {
        await _integrationDb.IntegrationEvents.AddAsync(new IntegrationEventLogEntry(@event));
        await _integrationDb.SaveChangesAsync();
    }
}
