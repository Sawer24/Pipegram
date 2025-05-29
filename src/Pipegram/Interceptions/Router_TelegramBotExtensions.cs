namespace Pipegram.Interceptions;

public static class Interceptor_TelegramBotExtensions
{
    public static ITelegramBot UseMessageInterceptor(this ITelegramBot bot)
    {
        bot.UseMiddleware<MessageInterceptorMiddleware>();
        return bot;
    }
}
