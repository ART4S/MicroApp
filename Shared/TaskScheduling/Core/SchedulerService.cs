using Microsoft.Extensions.DependencyInjection;
using TaskScheduling.Abstractions;

namespace TaskScheduling.Core;

internal class SchedulerService
{
    private readonly IServiceProvider _services;
    private readonly SchedulerSettings _settings;
    private readonly ICollection<RecurringBackgroundTask> _taskSettings;
    private readonly Action<Exception, IBackgroundTask, IServiceProvider> _exceptionHandler;

    public SchedulerService(
        IServiceProvider services,
        SchedulerSettings settings,
        ICollection<RecurringBackgroundTask> taskSettings,
        Action<Exception, IBackgroundTask, IServiceProvider> exceptioHandler)
    {
        _services = services;
        _settings = settings;
        _taskSettings = taskSettings;
        _exceptionHandler = exceptioHandler;
    }

    public async Task Run(CancellationToken stoppingToken)
    {
        do
        {
            var now = DateTime.UtcNow;

            var tasksToRun = _taskSettings.Where(x => x.NextRunTime <= now);

            if (!tasksToRun.Any())
                continue;

            var scope = _services.CreateScope();

            var tasksReadyToRun = tasksToRun
                .Select(x => scope.ServiceProvider.GetRequiredService(x.Type))
                .OfType<IBackgroundTask>();

            foreach (var task in tasksToRun)
            {
                var taskReadyToRun = (IBackgroundTask) scope.ServiceProvider.GetRequiredService(task.Type);

                Task.Run(async () =>
                {
                    try
                    {
                        await taskReadyToRun.Run(stoppingToken);
                    }
                    catch (Exception exception)
                    {
                        _exceptionHandler(exception, taskReadyToRun, _services);
                    }

                }); // Fire and forget

                task.CalculateNextRunTime();
            }
        } while (!await isStopped());

        async Task<bool> isStopped()
        {
            await Task.Delay(TimeSpan.FromSeconds(_settings.PollingIntervalSec), stoppingToken);
            return stoppingToken.IsCancellationRequested;
        }
    }
}
