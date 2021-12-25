using IdempotencyServices.Models;
using Microsoft.EntityFrameworkCore;

namespace IdempotencyServices.EF;

public class EFIdempotencyDbContext : DbContext, IEFIdempotencyDbContext
{
    public EFIdempotencyDbContext(DbContextOptions<EFIdempotencyDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("app");
    }

    public DbSet<ClientRequest> ClientRequests { get; set; }
}
