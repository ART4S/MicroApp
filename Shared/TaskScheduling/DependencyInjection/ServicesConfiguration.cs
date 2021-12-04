using TaskScheduling.Abstractions;
using TaskScheduling.Core;
using TaskScheduling.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesConfiguration
{
    public static void AddScheduler(
        this IServiceCollection services, 
        SchedulerSettings settings, 
        IEnumerable<BackgroundTaskSettings> taskSettings,
        Action<Exception, IBackgroundTask, IServiceProvider> exceptionHandler)
    {
        var tasks = taskSettings
            .Select(x => new RecurringBackgroundTask(x.Type, x.Schedule))
            .ToList();

        tasks.ForEach(x => services.AddScoped(x.Type));

        services.AddSingleton((sp) => ActivatorUtilities.CreateInstance<SchedulerService>(sp, settings, tasks, exceptionHandler));

        services.AddHostedService<SchedulerBackgroundService>();
    }
}
