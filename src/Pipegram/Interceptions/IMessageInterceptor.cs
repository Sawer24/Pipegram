namespace Pipegram.Interceptions;

public interface IMessageInterceptor
{
    IDisposable SetInterceptor(long chatId, UpdateDelegate interceptor);
    UpdateDelegate? UseInterceptor(long chatId);
}
