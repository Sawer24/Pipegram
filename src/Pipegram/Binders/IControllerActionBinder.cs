using Pipegram.Controllers;
using System.Reflection;

namespace Pipegram.Binders;

public interface IControllerActionBinder
{
    Func<TelegramControllerBase, UpdateContext, Task> CreateActionDelegate(Type controllerType, MethodInfo method);
}
