using IdempotencyServices.Model;
using Microsoft.EntityFrameworkCore;

namespace IdempotencyServices.EF;

public class IdempotencyDbContext : DbContext, IIdempotencyDbContext
{
    public IdempotencyDbContext(DbContextOptions<IdempotencyDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("app");
    }

    public DbSet<ClientRequest> ClientRequests { get; set; }
}
