namespace Pipegram.Controllers.Messages;

[AttributeUsage(AttributeTargets.Method)]
public class StartActionAttribute() : TextMessageActionAttribute("/start")
{
}
