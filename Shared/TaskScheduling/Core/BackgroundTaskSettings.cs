namespace TaskScheduling.Core;

public abstract record BackgroundTaskSettings
{
    private protected BackgroundTaskSettings(Type type, string schedule)
    {
        Type = type;
        Schedule = schedule;
    }

    public Type Type { get; }

    public string Schedule { get; }
}

public record BackgroundTaskSettings<TTask>(string Schedule) : BackgroundTaskSettings(typeof(TTask), Schedule);
