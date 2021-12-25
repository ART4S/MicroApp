using IdempotencyServices.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.DataAccess.Idempotency;

internal class IdempotencyDbContextDesignTimeFactory : IDesignTimeDbContextFactory<EFIdempotencyDbContext>
{
    public EFIdempotencyDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<EFIdempotencyDbContext> builder = new();

        builder.UseSqlServer("Server=.;Port=1433;Database=ordering_db;User Id=sa;Password=Qwerty123", options =>
        {
            options.MigrationsAssembly(GetType().Assembly.FullName);
        });

        return new EFIdempotencyDbContext(builder.Options);
    }
}
