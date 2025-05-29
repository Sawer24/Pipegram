namespace Pipegram.Interceptions;

public class MessageInterceptor : IMessageInterceptor
{
    private readonly Dictionary<long, UpdateDelegate> _interceptors = [];

    public IDisposable SetInterceptor(long chatId, UpdateDelegate interceptor)
    {
        lock (_interceptors)
            _interceptors[chatId] = interceptor;
        return new DisposableAction(() =>
        {
            lock (_interceptors)
                if (_interceptors.TryGetValue(chatId, out var actualInterceptor)
                    && actualInterceptor == interceptor)
                    _interceptors.Remove(chatId);
        });
    }

    public UpdateDelegate? UseInterceptor(long chatId)
    {
        lock (_interceptors)
        {
            if (_interceptors.TryGetValue(chatId, out var interceptor))
                _interceptors.Remove(chatId);
            return interceptor;
        }
    }

    private class DisposableAction(Action action) : IDisposable
    {
        private Action? _action = action;

        public void Dispose()
        {
            _action?.Invoke();
            _action = null;
        }
    }
}
