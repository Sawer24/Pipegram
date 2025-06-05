namespace Pipegram;

public interface ITelegramApplication : IPipelineBuilder
{
    IServiceProvider Services { get; }

    Task RunAsync(CancellationToken stoppingToken = default);
}
