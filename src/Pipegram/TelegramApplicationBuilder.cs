using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Pipegram;

public class TelegramApplicationBuilder : ITelegramApplicationBuilder
{
    public IServiceCollection Services { get; } = new ServiceCollection();

    public static TelegramApplicationBuilder CreateBuilder(TelegramBotClientOptions options) => new(options);

    public TelegramApplicationBuilder(TelegramBotClientOptions options)
    {
        Services.AddLogging(b => b.AddConsole());
        Services.AddTelegramBot(options);
    }

    public ITelegramApplication Build()
    {
        var provider = Services.BuildServiceProvider();
        return new TelegramApplication(provider);
    }
}
