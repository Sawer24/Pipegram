namespace Pipegram;

public static class TelegramApplicationExtensions
{
    public static void Run(this ITelegramApplication application)
    {
        application.RunAsync().GetAwaiter().GetResult();
    }
}
