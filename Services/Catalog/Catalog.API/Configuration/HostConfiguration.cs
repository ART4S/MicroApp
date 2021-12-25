using Catalog.API.DataAccess;
using Catalog.API.Settings;
using HostConfiguration;
using IntegrationServices.EF;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Configuration;

static class HostConfiguration
{
    public static IHost InitCatalogDb(this IHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();

        bool createDb = config.GetValue<bool>("ClearDatabase");

        if (createDb)
        {
            var settings = host.Services.GetRequiredService<IOptions<CatalogDbSettings>>().Value;

            MongoClient client = new(settings.ConnectionString);

            client.DropDatabase(settings.DatabaseName);

            IServiceScope scope = host.Services.CreateScope();
            var seeder = ActivatorUtilities.CreateInstance<CatalogDbContextSeed>(scope.ServiceProvider);
            seeder.Seed().Wait();
        }

        return host;
    }

    public static IHost InitIntegrationDb(this IHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();

        bool createDb = config.GetValue<bool>("ClearDatabase");

        if (createDb)
            host.CreateDbContext<EFIntegrationDbContext>();
        else
            host.MigrateDbContext<EFIntegrationDbContext>();

        return host;
    }
}
