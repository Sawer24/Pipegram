namespace Pipegram.Controllers;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class MessageActionAttribute(string name) : ActionAttribute(name)
{
}
