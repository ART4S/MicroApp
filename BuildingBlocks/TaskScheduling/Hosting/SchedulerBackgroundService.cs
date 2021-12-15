using Microsoft.Extensions.Hosting;
using TaskScheduling.Core;

namespace TaskScheduling.Hosting;

internal class SchedulerBackgroundService : BackgroundService
{
    private readonly SchedulerService _scheduler;

    public SchedulerBackgroundService(SchedulerService scheduler)
    {
        _scheduler = scheduler;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return _scheduler.Run(stoppingToken);
    }
}