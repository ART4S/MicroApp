using IntegrationServices.DataAccess;
using IntegrationServices.Entities;

namespace IntegrationServices;

public class IntegrationEventService : IIntegrationEventService
{
    private readonly IIntegrationDbContext _integrationDb;

    public IntegrationEventService(IIntegrationDbContext integrationDb)
    {
        _integrationDb = integrationDb;
    }

    public async Task Save(IntegrationEvent @event)
    {
        await _integrationDb.IntegrationEvents.AddAsync(new IntegrationEventLogEntry(@event));
        await _integrationDb.SaveChangesAsync();
    }
}
