namespace Pipegram.Interceptions;

public static class MessageInterceptorExtensions
{
    public static async Task<IMessageInterceptResult> InterceptMessage(this IMessageInterceptor interceptor,
        long chatId, int? messageId = null, int? timeoutInMilliseconds = 60 * 60 * 1000)
    {
        var cancellationToken = new CancellationTokenSource();
        MessageInterceptResult result = new();
        var interception = interceptor.SetInterceptor(chatId,
            message =>
            {
                result.TrySetMessage(message);
                if (result.IsMessage)
                    cancellationToken.Cancel();
                return true;
            },
            callbackQuery =>
            {
                if (!messageId.HasValue || callbackQuery.Message?.Id != messageId.Value)
                    return false;
                result.TrySetCallbackQuery(callbackQuery);
                if (result.IsCallbackQuery)
                    cancellationToken.Cancel();
                return true;
            },
            () =>
            {
                result.TrySetInterrupted();
                if (result.IsInterrupted)
                    cancellationToken.Cancel();
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
