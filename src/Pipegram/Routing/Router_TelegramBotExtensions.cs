namespace Pipegram.Routing;

public static class Router_TelegramBotExtensions
{
    public static ITelegramApplication UseRouting(this ITelegramApplication application)
    {
        application.UseMiddleware<EndpointRoutingMiddleware>();
        return application;
    }

    public static ITelegramApplication UseEndpoints(this ITelegramApplication application)
    {
        application.UseMiddleware<EndpointMiddleware>();
        return application;
    }
}
