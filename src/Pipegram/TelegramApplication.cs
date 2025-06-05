using Microsoft.Extensions.DependencyInjection;

namespace Pipegram;

public class TelegramApplication(IServiceProvider services) : ITelegramApplication
{
    private readonly ITelegramBot _telegramBot = services.GetRequiredService<ITelegramBot>();
    private readonly PipelineBuilder _builder = new();

    public IServiceProvider Services { get; } = services;

    public IPipelineBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware) => _builder.Use(middleware);

    UpdateDelegate IPipelineBuilder.Build() => _builder.Build();

    public Task RunAsync(CancellationToken stoppingToken = default)
        => _telegramBot.Use(_builder.Build()).RunAsync(stoppingToken);
}
