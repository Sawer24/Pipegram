namespace Pipegram;

public interface ITelegramBot
{
    ITelegramBot Use(UpdateDelegate pipeline);

    Task RunAsync(CancellationToken stoppingToken = default);
}
