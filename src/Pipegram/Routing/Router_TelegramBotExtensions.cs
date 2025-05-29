namespace Pipegram.Routing;

public static class Router_TelegramBotExtensions
{
    public static ITelegramBot UseRouting(this ITelegramBot bot)
    {
        bot.UseMiddleware<EndpointRoutingMiddleware>();
        return bot;
    }

    public static ITelegramBot UseEndpoints(this ITelegramBot bot)
    {
        bot.UseMiddleware<EndpointMiddleware>();
        return bot;
    }
}
