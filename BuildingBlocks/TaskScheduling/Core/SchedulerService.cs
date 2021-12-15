using Microsoft.Extensions.DependencyInjection;
using TaskScheduling.Abstractions;

namespace TaskScheduling.Core;

internal class SchedulerService
{
    private readonly IServiceProvider _services;
    private readonly SchedulerSettings _settings;
    private readonly ICollection<RecurringBackgroundTask> _taskSettings;
    private readonly Action<Exception, IBackgroundTask?, IServiceProvider> _exceptionHandler;

    public SchedulerService(
        IServiceProvider services,
        SchedulerSettings settings,
        ICollection<RecurringBackgroundTask> taskSettings,
        Action<Exception, IBackgroundTask?, IServiceProvider> exceptioHandler)
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

            foreach (var task in tasksToRun)
            {
                if (!TryCreateTask(task, scope.ServiceProvider, out IBackgroundTask taskReadyToRun))
                    continue;

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

    private bool TryCreateTask(
        RecurringBackgroundTask taskDescription, 
        IServiceProvider services, 
        out IBackgroundTask task)
    {
        task = null;

        try
        {
            task = (IBackgroundTask)(taskDescription.Factory is not null
                ? taskDescription.Factory(services)
                : ActivatorUtilities.CreateInstance(services, taskDescription.Type));

            return true;
        }
        catch(Exception exeption)
        {
            _exceptionHandler?.Invoke(exeption, task, services);
        }

        return false;
    }
}
