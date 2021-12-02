using NCrontab;

namespace TaskScheduling;

internal record ScheduledBackgroundTask
{
    private readonly CrontabSchedule _schedule;

    public ScheduledBackgroundTask(Type type, string cronSchedule)
    {
        _schedule = CrontabSchedule.Parse(cronSchedule);
        Type = type;
        CalculateNextRunTime();
    }

    public Type Type { get; }

    public DateTime NextRunTime { get; private set; }

    public void CalculateNextRunTime()
    {
        NextRunTime = _schedule.GetNextOccurrence(DateTime.Now);
    }
}
