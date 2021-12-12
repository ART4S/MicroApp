using IdempotencyServices.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IdempotencyServices.EF;

public interface IIdempotencyDbContext
{
    DatabaseFacade Database { get; }

    DbSet<ClientRequest> ClientRequests { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
