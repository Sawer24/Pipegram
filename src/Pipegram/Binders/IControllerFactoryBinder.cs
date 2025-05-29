using Pipegram.Controllers;

namespace Pipegram.Binders;

public interface IControllerFactoryBinder
{
    Func<UpdateContext, TelegramControllerBase> CreateFactory(Type controllerType);
}
