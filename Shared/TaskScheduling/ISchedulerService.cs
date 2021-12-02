namespace TaskScheduling;

internal interface ISchedulerService
{
    Task Run(CancellationToken stoppingToken);
}
