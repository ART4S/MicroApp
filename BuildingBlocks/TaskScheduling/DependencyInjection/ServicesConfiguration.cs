using Microsoft.Extensions.DependencyInjection;
using TaskScheduling.Abstractions;
using TaskScheduling.Core;
using TaskScheduling.Hosting;

namespace TaskScheduling.DependencyInjection;

public static class ServicesConfiguration
{
    public static void AddScheduler(
        this IServiceCollection services,
        SchedulerSettings settings,
        IEnumerable<BackgroundTaskSettings> taskSettings,
        Action<Exception, IBackgroundTask, IServiceProvider> exceptionHandler)
    {
        var tasks = taskSettings
            .Select(x => new RecurringBackgroundTask(x.Type, x.Schedule, x.Factory))
            .ToList();

        services.AddSingleton((sp) => ActivatorUtilities.CreateInstance<SchedulerService>(sp, settings, tasks, exceptionHandler));

        services.AddHostedService<SchedulerBackgroundService>();
    }
}
