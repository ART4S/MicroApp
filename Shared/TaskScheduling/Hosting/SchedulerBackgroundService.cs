using Microsoft.Extensions.Hosting;

namespace TaskScheduling.Hosting;

internal class SchedulerBackgroundService : BackgroundService
{
    private readonly ISchedulerService _scheduler;

    public SchedulerBackgroundService(ISchedulerService scheduler)
    {
        _scheduler = scheduler;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return _scheduler.Run(stoppingToken);
    }
}