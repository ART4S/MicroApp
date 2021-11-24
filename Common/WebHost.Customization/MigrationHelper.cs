using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System.Data.Common;

namespace Microsoft.AspNetCore.Hosting;

public static class MigrationHelper
{
    public static void MigrateDbContext<TDbContext>(
        this IWebHost host, Action<IServiceProvider, TDbContext> seedAction = null, bool newDb = false) where TDbContext : DbContext
    {
        using IServiceScope scope = host.Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TDbContext>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        const int retries = 3;

        Policy policy = Policy.Handle<DbException>().WaitAndRetry(
            retryCount: retries,
            sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            onRetry: (exception, _, attempt, _) =>
            {
                // TODO: log
            });

        try
        {
            // TODO: log

            policy.Execute(() =>
            {
                if (newDb) dbContext.Database.EnsureDeleted();

                dbContext.Database.Migrate();

                if (seedAction is not null)
                    seedAction(scope.ServiceProvider, dbContext);
            });
        }
        catch (Exception ex)
        {
            // TODO: log
            throw;
        }
    }
}
