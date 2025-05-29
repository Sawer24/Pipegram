namespace Pipegram.Controllers;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CallbackQueryActionAttribute(string name) : ActionAttribute(name)
{
}
