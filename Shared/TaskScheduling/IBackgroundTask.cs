namespace TaskScheduling;

public interface IBackgroundTask
{
    Task Run(CancellationToken cancellationToken);
}
