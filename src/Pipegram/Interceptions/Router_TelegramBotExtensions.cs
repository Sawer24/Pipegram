namespace Pipegram.Interceptions;

public static class Interceptor_TelegramBotExtensions
{
    public static ITelegramBot UseInterceptors(this ITelegramBot bot)
    {
        bot.UseMiddleware<MessageInterceptorMiddleware>();
        bot.UseMiddleware<CallbackQueryInterceptorMiddleware>();
        return bot;
    }
}
