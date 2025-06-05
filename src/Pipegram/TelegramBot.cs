using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Pipegram;

public class TelegramBot(TelegramBotClientOptions options,
    IServiceProvider services, ILogger<TelegramBot> logger) : ITelegramBot
{
    private readonly IServiceProvider _services = services;
    private readonly TelegramBotClientOptions _options = options;
    private readonly ILogger<TelegramBot> _logger = logger;

    private UpdateDelegate? _pipeline;

    private TelegramBotClient? _client;
    private User? _botUser;

    public ITelegramBot Use(UpdateDelegate pipeline)
    {
        if (_client is not null)
            throw new InvalidOperationException("Middleware pipeline cannot be set after the bot has started.");
        _pipeline = pipeline;
        return this;
    }

    public async Task RunAsync(CancellationToken stoppingToken = default)
    {
        if (_pipeline is null)
            throw new InvalidOperationException("Middleware pipeline is not set. Use Use() method to set it.");
        _client = new TelegramBotClient(_options, cancellationToken: stoppingToken);
        _client.OnUpdate += Client_OnUpdate;
        _client.OnError += Client_OnError;
        _botUser = await _client.GetMe(cancellationToken: stoppingToken);
        await Task.Delay(-1, stoppingToken);
    }

    private Task Client_OnUpdate(Update update)
    {
        _ = Task.Run(async () =>
        {
            await using var scope = _services.CreateAsyncScope();
            var context = new UpdateContext
            {
                Update = update,
                BotClient = _client!,
                BotUser = _botUser!,
                Services = scope.ServiceProvider,
                CancellationToken = _client!.GlobalCancelToken
            };
            try
            {
                await _pipeline!.Invoke(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in middleware pipeline while handling update.");
            }
        }, _client!.GlobalCancelToken);
        return Task.CompletedTask;
    }

    private Task Client_OnError(Exception exception, HandleErrorSource source)
    {
        _logger.LogError(exception, "Client '{source}' exception.", source);
        return Task.CompletedTask;
    }
}
