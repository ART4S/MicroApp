using Catalog.API.DataAccess;
using Catalog.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Configuration;

static class HostConfiguration
{
    public static IHost InitCatalogDb(this IHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();

        bool clearDb = config.GetValue<bool>("ClearDatabase");

        if (clearDb)
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
}
