using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Pipegram.Controllers;

public static class TelegramController_TelegramBotExtensions
{
    public static ITelegramApplication MapControllers(this ITelegramApplication application)
    {
        var assembly = Assembly.GetCallingAssembly();

        var registrationEngine = application.Services.GetRequiredService<IEndpointRegistrationEngine>();
        var controllerTypes = assembly.GetTypes()
            .Where(t => typeof(ITelegramController).IsAssignableFrom(t) && !t.IsAbstract);
        foreach (var controllerType in controllerTypes)
            registrationEngine.RegisterAllEndpoints(controllerType);
        return application;
    }

    public static ITelegramApplication MapController<TController>(this ITelegramApplication application)
        => application.MapController(typeof(TController));

    public static ITelegramApplication MapController(this ITelegramApplication application, Type controllerType)
    {
        var registrationEngine = application.Services.GetRequiredService<IEndpointRegistrationEngine>();

        registrationEngine.RegisterAllEndpoints(controllerType);
        return application;
    }
}
