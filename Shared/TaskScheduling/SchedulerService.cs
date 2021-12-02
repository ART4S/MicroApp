using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TaskScheduling;

internal class SchedulerService : ISchedulerService
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IServiceProvider _services;
    private readonly SchedulerSettings _settings;
    private readonly ICollection<ScheduledBackgroundTask> _taskSettings;

    public SchedulerService(
        ILoggerFactory loggerFactory,
        IServiceProvider services, 
        SchedulerSettings settings,
        ICollection<ScheduledBackgroundTask> taskSettings)
    {
        _loggerFactory = loggerFactory;
        _services = services;
        _settings = settings;
        _taskSettings = taskSettings;
    }

    public async Task Run(CancellationToken stoppingToken)
    {
        do
        {
            var now = DateTime.Now;

            var tasksToRun = _taskSettings.Where(x => x.NextRunTime <= now);

            if (!tasksToRun.Any())
                continue;

            var scope = _services.CreateScope();

            var tasksReadyToRun = tasksToRun
                .Select(x => scope.ServiceProvider.GetRequiredService(x.Type))
                .OfType<IBackgroundTask>();

            foreach (var task in tasksToRun)
            {
                var taskReadyToRun = scope.ServiceProvider.GetRequiredService(task.Type) as IBackgroundTask;

                Task.Run(() => ExecuteTask(taskReadyToRun, stoppingToken)); // Fire and forget

                task.CalculateNextRunTime();
            }
        } while (!await IsCancelled());

        async Task<bool> IsCancelled()
        {
            await Task.Delay(TimeSpan.FromSeconds(_settings.PoolingIntervalSec), stoppingToken);
            return stoppingToken.IsCancellationRequested;
        }
    }

    private async Task ExecuteTask(IBackgroundTask task, CancellationToken cancellationToken)
    {
        try
        {
            await task.Run(cancellationToken);
        }
        catch (Exception ex)
        {
            ILogger logger = _loggerFactory.CreateLogger(task.GetType());

            logger.LogError(ex, "Error occured while executing task");
        }
    }
}
