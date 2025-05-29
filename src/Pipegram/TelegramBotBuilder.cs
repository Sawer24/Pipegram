using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Pipegram;

public class TelegramBotBuilder
{
    public IServiceCollection Services { get; } = new ServiceCollection();

    public static TelegramBotBuilder CreateBuilder(TelegramBotClientOptions options) => new(options);

    public TelegramBotBuilder(TelegramBotClientOptions options)
    {
        Services.AddLogging(b => b.AddConsole());
        Services.AddTelegramBot(options);
    }

    public ITelegramBot Build()
    {
        var provider = Services.BuildServiceProvider();
        return provider.GetRequiredService<ITelegramBot>();
    }
}
