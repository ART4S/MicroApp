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
        while(!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;

            var tasksToRun = _taskSettings.Where(x => x.NextRunTime <= now);

            if (!tasksToRun.Any())
            {
                await Task.Delay(TimeSpan.FromSeconds(_settings.PollingIntervalSec), stoppingToken);
                continue;
            }

            var scope = _services.CreateScope();

            foreach (var task in tasksToRun)
                await HandleTask(task);
            
            await Task.Delay(TimeSpan.FromSeconds(_settings.PollingIntervalSec), stoppingToken);
        }
    }
    
    private async Task HandleTask(RecurringBackgroundTask task, CancellationToken stoppingToken)
    {
        task.CalculateNextRunTime();
        
        using var scope = _services.CreateScope();

        try
        {
            var taskObj = (IBackgroundTask)(taskDescription.Factory is not null
                ? taskDescription.Factory(scope.ServiceProvider)
                : ActivatorUtilities.CreateInstance(scope.ServiceProvider, taskDescription.Type));
            
            await taskObj.Run(stoppingToken);
        }
        catch(Exception ex)
        {
            _exceptionHandler.Invoke(ex, task, scope.ServiceProvider);
        }
    }
}
