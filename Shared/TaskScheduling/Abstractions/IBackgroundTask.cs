namespace TaskScheduling.Abstractions;

public interface IBackgroundTask
{
    Task Run(CancellationToken cancellationToken);
}
