using HostConfiguration;
using IdempotencyServices.EF;
using IntegrationServices.EF;
using Ordering.Infrastructure.DataAccess.Ordering;

namespace Ordering.API.Configuration;

static class HostConfiguration
{
    public static IHost MigrateOrderingDbContext(this IHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();

        bool createDb = config.GetValue<bool>("CreateNewDb");

        if (createDb)
            host.MigrateDbContext<OrderingDbContext>((services, db) => new OrderingDbContextSeed(services, db).Seed());
        else
            host.MigrateDbContext<OrderingDbContext>();

        return host;
    }

    public static IHost MigrateIntegrationDbContext(this IHost host)
    {
        host.MigrateDbContext<IntegrationDbContext>();
        return host;
    }

    public static IHost MigrateIdempotencyDbContext(this IHost host)
    {
        host.MigrateDbContext<IdempotencyDbContext>();
        return host;
    }
}
