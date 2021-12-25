using IntegrationServices.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.DataAccess.Integration;

internal class IntegrationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<EFIntegrationDbContext>
{
    public EFIntegrationDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<EFIntegrationDbContext> builder = new();

        builder.UseSqlServer("Server=.;Port=1433;Database=ordering_db;User Id=sa;Password=Qwerty123", options =>
        {
            options.MigrationsAssembly(GetType().Assembly.FullName);
        });

        return new EFIntegrationDbContext(builder.Options);
    }
}
