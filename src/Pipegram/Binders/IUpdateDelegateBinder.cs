namespace Pipegram.Binders;

public interface IActionUpdateDelegateBinder
{
    UpdateDelegate CreateActionUpdateDelegate(Delegate handler);
}
