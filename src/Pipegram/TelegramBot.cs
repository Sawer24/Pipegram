using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Pipegram;

public class TelegramBot(TelegramBotClientOptions options,
    IServiceProvider services, ILogger<TelegramBot> logger) : ITelegramBot
{
    private const string UpdateUnhandledKey = "__UpdateUnhandled";

    private readonly TelegramBotClientOptions _options = options;
    private readonly ILogger<TelegramBot> _logger = logger;

    private readonly List<Func<UpdateDelegate, UpdateDelegate>> _components = [];
    private UpdateDelegate? _pipeline;
    private TelegramBotClient? _client;

    public IServiceProvider Services { get; } = services;
    public ITelegramBotClient? Client => _client;
    public User? BotUser { get; private set; }

    public ITelegramBot Use(Func<UpdateDelegate, UpdateDelegate> middleware)
    {
        _components.Add(middleware);
        return this;
    }

    public async Task RunAsync(CancellationToken stoppingToken = default)
    {
        _pipeline = BuildPipeline();
        _client = new TelegramBotClient(_options, cancellationToken: stoppingToken);
        _client.OnUpdate += Client_OnUpdate;
        _client.OnError += Client_OnError;
        BotUser = await _client.GetMe(cancellationToken: stoppingToken);
        await Task.Delay(-1, stoppingToken);
    }

    private UpdateDelegate BuildPipeline()
    {
        UpdateDelegate pipeline = context =>
        {
            context.Items[UpdateUnhandledKey] = true;
            return Task.CompletedTask;
        };

        for (var c = _components.Count - 1; c >= 0; c--)
            pipeline = _components[c](pipeline);

        return pipeline;
    }

    private Task Client_OnUpdate(Update update)
    {
        _ = Task.Run(async () =>
        {
            await using var scope = Services.CreateAsyncScope();
            var context = new UpdateContext
            {
                TelegramBot = this,
                Update = update,
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
