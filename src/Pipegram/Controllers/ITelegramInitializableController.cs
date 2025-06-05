namespace Pipegram.Controllers;

public interface ITelegramInitializableController : ITelegramController
{
    public ITelegramController Initialize(UpdateContext context);
}
