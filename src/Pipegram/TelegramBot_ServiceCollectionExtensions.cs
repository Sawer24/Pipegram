using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Pipegram;

public static class TelegramBot_ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, TelegramBotClientOptions options)
    {
        services.AddSingleton(options);
        services.AddSingleton<ITelegramBot, TelegramBot>();
        return services;
    }
}
