using Telegram.Bot.Types.Enums;

namespace Pipegram.Routing;

public class RouterDictionary : Dictionary<UpdateType, IRouter>, IRouterDictionary
{
}
