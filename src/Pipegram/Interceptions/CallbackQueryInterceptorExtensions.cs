namespace Pipegram.Interceptions;

public static class CallbackQueryInterceptorExtensions
{
    public static async Task<ICallbackQueryInterceptResult> InterceptCallbackQuery(this ICallbackQueryInterceptor interceptor,
        long chatId, int messageId, int? timeoutInMilliseconds = 60 * 60 * 1000)
    {
        var cancellationToken = new CancellationTokenSource();
        CallbackQueryInterceptResult result = new();
        var interception = interceptor.SetInterceptor(chatId, messageId,
            callbackQuery =>
            {
                result.TrySetCallbackQuery(callbackQuery);
                if (result.IsCallbackQuery)
                    cancellationToken.Cancel();
                return true;
            });
        try
        {
            await Task.Delay(timeoutInMilliseconds ?? -1, cancellationToken.Token);
            result.TrySetTimeout();
            if (result.IsTimeout)
                interceptor.CancelInterceptor(interception);
        }
        catch (TaskCanceledException)
        {
        }
        return result;
    }
}
