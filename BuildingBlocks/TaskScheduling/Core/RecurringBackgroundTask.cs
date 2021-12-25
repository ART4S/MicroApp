using NCrontab;
using TaskScheduling.Abstractions;

namespace TaskScheduling.Core;

internal record RecurringBackgroundTask
{
    private readonly CrontabSchedule _schedule;

    public RecurringBackgroundTask(
        Type type, 
        string cronSchedule, 
        Func<IServiceProvider, IBackgroundTask>? factory = null)
    {
        _schedule = CrontabSchedule.Parse(cronSchedule, new() { IncludingSeconds = true });
        Type = type;
        Factory = factory;
        CalculateNextRunTime();
    }

    public Type Type { get; }

    public DateTime NextRunTime { get; private set; }

    public Func<IServiceProvider, IBackgroundTask>? Factory { get; }

    public void CalculateNextRunTime()
    {
        NextRunTime = _schedule.GetNextOccurrence(DateTime.UtcNow);
    }
}
