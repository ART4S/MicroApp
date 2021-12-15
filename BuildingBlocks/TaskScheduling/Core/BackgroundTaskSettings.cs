using TaskScheduling.Abstractions;

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

    internal Func<IServiceProvider, IBackgroundTask>? Factory { get; init; }
}

public record BackgroundTaskSettings<TTask>(string Schedule) : BackgroundTaskSettings(typeof(TTask), Schedule)
    where TTask : IBackgroundTask
{
    public new Func<IServiceProvider, TTask> Factory
    {
        init => base.Factory = (sp) => value(sp);
    }
}
