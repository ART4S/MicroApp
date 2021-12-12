using IdempotencyServices.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.DataAccess.Idempotency;

internal class IdempotencyDbContextDesignTimeFactory : IDesignTimeDbContextFactory<IdempotencyDbContext>
{
    public IdempotencyDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<IdempotencyDbContext> builder = new();

        builder.UseSqlServer("Server=.;Port=1433;Database=ordering_db;User Id=sa;Password=Qwerty123", options =>
        {
            options.MigrationsAssembly(GetType().Assembly.FullName);
        });

        return new IdempotencyDbContext(builder.Options);
    }
}
