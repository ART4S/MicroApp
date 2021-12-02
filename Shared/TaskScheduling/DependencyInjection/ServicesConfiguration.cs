using TaskScheduling;
using TaskScheduling.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesConfiguration
{
    public static void AddScheduler(
        this IServiceCollection services, 
        SchedulerSettings settings, 
        IEnumerable<BackgroundTaskSettings> taskSettings)
    {
        var tasks = taskSettings
            .Select(x => new ScheduledBackgroundTask(x.Type, x.Schedule))
            .ToList();

        tasks.ForEach(x => services.AddScoped(x.Type));

        services.AddSingleton<ISchedulerService, SchedulerService>(
            (sp) => ActivatorUtilities.CreateInstance<SchedulerService>(sp, settings, tasks));

        services.AddHostedService<SchedulerBackgroundService>();
    }
}
