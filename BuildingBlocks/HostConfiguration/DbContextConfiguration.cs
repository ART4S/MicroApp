using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System.Data.Common;

namespace HostConfiguration;

public static class DbContextConfiguration
{
    public static void MigrateDbContext<TDbContext>(
        this IHost host,
        Action<IServiceProvider, TDbContext>? seedAction = null)
        where TDbContext : DbContext
    {
        PerformMigration(host, seedAction);
    }

    public static void CreateDbContext<TDbContext>(
        this IHost host,
        Action<IServiceProvider, TDbContext>? seedAction = null)
        where TDbContext : DbContext
    {
        PerformMigration(host, seedAction, create: true);
    }

    private static void PerformMigration<TDbContext>(
        IHost host, 
        Action<IServiceProvider, TDbContext>? seedAction = null,
        bool create = false) 
        where TDbContext : DbContext
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
                logger.LogError(exception, "Error occured while performing migrations on attempt {Attempt}", attempt);
            });

        try
        {
            logger.LogInformation("Start performing migrations");

            policy.Execute(() =>
            {
                if (create) 
                    dbContext.Database.EnsureDeleted();

                dbContext.Database.Migrate();

                if (seedAction is not null)
                    seedAction(scope.ServiceProvider, dbContext);
            });
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Performing migrations failed");
            throw;
        }
    }
}
