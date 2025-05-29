using Telegram.Bot.Types.Enums;

namespace Pipegram.Routing;

public interface IRouterDictionary : IDictionary<UpdateType, IRouter>
{
}
