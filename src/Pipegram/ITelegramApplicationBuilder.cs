using Microsoft.Extensions.DependencyInjection;

namespace Pipegram;

public interface ITelegramApplicationBuilder
{
    IServiceCollection Services { get; }

    ITelegramApplication Build();
}
