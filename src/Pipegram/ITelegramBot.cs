using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pipegram;

public interface ITelegramBot
{
    IServiceProvider Services { get; }
    ITelegramBotClient? Client { get; }
    User? BotUser { get; }

    ITelegramBot Use(Func<UpdateDelegate, UpdateDelegate> middleware);

    Task RunAsync(CancellationToken stoppingToken = default);
}