using Catalog.Infrastructure.DataAccess.Catalog;
using HostConfiguration;
using IntegrationServices.EF;

namespace Catalog.API.Configuration;

static class HostConfiguration
{
    public static IHost MigrateCatalogDbContext(this IHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();

        bool createDb = config.GetValue<bool>("CreateNewDb");

        if (createDb)
            host.CreateDbContext<CatalogDbContext>((services, context) => 
                new CatalogDbContextSeed(services, context).Seed());
        else
            host.MigrateDbContext<CatalogDbContext>();

        return host;
    }

    public static IHost MigrateIntegrationDbContext(this IHost host)
    {
        host.MigrateDbContext<IntegrationDbContext>();
        return host;
    }
}
