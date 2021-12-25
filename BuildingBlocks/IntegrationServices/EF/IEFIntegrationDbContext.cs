using IntegrationServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IntegrationServices.EF;

public interface IEFIntegrationDbContext
{
    DatabaseFacade Database { get; }
    DbSet<IntegrationEventLogEntry> IntegrationEvents { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
