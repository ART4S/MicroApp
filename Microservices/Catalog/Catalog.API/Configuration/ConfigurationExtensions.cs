using Catalog.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Configuration;

static class ConfigurationExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "MicroApp - Catalog.API",
                Version = "v1"
            });
        });
    }

    public static void AddCatalogDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOpt =>
            {
                sqlOpt.EnableRetryOnFailure();
            });
        });
    }

    public static IWebHost MigrateCatalogDbContext(this IWebHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();

        bool newDb = false;

        if (!string.IsNullOrEmpty(config["CreateNewDb"]))
            newDb = config.GetSection("CreateNewDb").Get<bool>();

        if (newDb)
        {
            host.MigrateDbContext<CatalogDbContext>(
                seedAction: (services, dbContext) => new CatalogDbContextSeed(services, dbContext).Seed(),
                newDb: true);
        }
        else
            host.MigrateDbContext<CatalogDbContext>(newDb: false);

        return host;
    }
}