using NCrontab;

namespace TaskScheduling.Core;

internal record RecurringBackgroundTask
{
    private readonly CrontabSchedule _schedule;

    public RecurringBackgroundTask(Type type, string cronSchedule)
    {
        _schedule = CrontabSchedule.Parse(cronSchedule);
        Type = type;
        CalculateNextRunTime();
    }

    public Type Type { get; }

    public DateTime NextRunTime { get; private set; }

    public void CalculateNextRunTime()
    {
        NextRunTime = _schedule.GetNextOccurrence(DateTime.UtcNow);
    }
}
