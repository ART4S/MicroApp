using IntegrationServices.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IntegrationServices.DataAccess;

public interface IIntegrationDbContext
{
    DatabaseFacade Database { get; }

    DbSet<IntegrationEventLogEntry> IntegrationEvents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
