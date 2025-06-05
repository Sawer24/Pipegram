using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pipegram.Interceptions;

public static class Interceptor_ServiceCollectionExtensions
{
    public static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.TryAddSingleton<IMessageInterceptor, MessageInterceptor>();
        services.TryAddSingleton<ICallbackQueryInterceptor, CallbackQueryInterceptor>();
        return services;
    }
}
