using IntegrationServices.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.DataAccess.Integration;

internal class IntegrationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<IntegrationDbContext>
{
    public IntegrationDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<IntegrationDbContext> builder = new();

        builder.UseSqlServer("Server=.;Port=1433;Database=ordering_db;User Id=sa;Password=Qwerty123", options =>
        {
            options.MigrationsAssembly(GetType().Assembly.FullName);
        });

        return new IntegrationDbContext(builder.Options);
    }
}
