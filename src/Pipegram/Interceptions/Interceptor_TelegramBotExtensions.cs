namespace Pipegram.Interceptions;

public static class Interceptor_TelegramBotExtensions
{
    public static ITelegramApplication UseInterceptors(this ITelegramApplication application)
    {
        application.UseMiddleware<MessageInterceptorMiddleware>();
        application.UseMiddleware<CallbackQueryInterceptorMiddleware>();
        return application;
    }
}
