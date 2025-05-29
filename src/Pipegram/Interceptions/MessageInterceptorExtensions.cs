using Telegram.Bot.Types;

namespace Pipegram.Interceptions;

public static class MessageInterceptorExtensions
{
    public static async Task<Message?> InterceptMessage(this IMessageInterceptor interceptor,
        long chatId, int timeoutInMilliseconds = 60 * 60 * 1000)
    {
        var cancellationToken = new CancellationTokenSource();
        Message? result = null;
        var interception = interceptor.SetInterceptor(chatId, context =>
        {
            result = context.Update.Message;
            cancellationToken.Cancel();
            return Task.CompletedTask;
        });
        try
        {
            await Task.Delay(timeoutInMilliseconds, cancellationToken.Token);
            interception.Dispose();
        }
        catch (TaskCanceledException)
        {
        }
        return result;
    }
}
