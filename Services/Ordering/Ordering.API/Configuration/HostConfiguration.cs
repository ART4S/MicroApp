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

        bool clearDb = config.GetValue<bool>("ClearDatabase");

        if (clearDb)
            host.CreateDbContext<OrderingDbContext>((services, context) => 
                new OrderingDbContextSeed(services, context).Seed());
        else
            host.MigrateDbContext<OrderingDbContext>();

        return host;
    }

    public static IHost MigrateIntegrationDbContext(this IHost host)
    {
        host.MigrateDbContext<EFIntegrationDbContext>();
        return host;
    }

    public static IHost MigrateIdempotencyDbContext(this IHost host)
    {
        host.MigrateDbContext<EFIdempotencyDbContext>();
        return host;
    }
}
