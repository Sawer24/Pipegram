namespace Pipegram.Controllers;

public interface IControllerFactoryBinder
{
    Func<UpdateContext, object> CreateFactory(Type controllerType);
}
