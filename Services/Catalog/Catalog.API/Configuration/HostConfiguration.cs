using Catalog.Infrastructure.DataAccess.Catalog;
using HostConfiguration;
using IntegrationServices.DataAccess;

namespace Catalog.API.Configuration;

static class HostConfiguration
{
    public static IHost MigrateCatalogDbContext(this IHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();

        bool createDb = false;

        var section = config.GetSection("CreateNewDb");
        if (section.Exists())
            createDb = section.Get<bool>();

        if (createDb)
            host.MigrateDbContext<CatalogDbContext>((services, db) => new CatalogDbContextSeed(services, db).Seed());
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
