using Mvc.Models;
using Pipegram.Routing;

namespace Mvc.Views;

public class ItemsListView : ViewBase
{
    public ItemsListView(IEnumerable<Item> items)
    {
        var any = false;
        foreach (var item in items)
        {
            keyboard.AddNewRow((item.Name, Endpoints.Items.Item, [item.Id]));
            any = true;
        }
        if (any)
            text.Append("Items List:");
        else
            text.Append("No items.");

        keyboard.AddNewRow(("Create item", Endpoints.Items.Create));
        keyboard.AddNewRow(("Back to menu", Endpoints.Home.Start));
    }
}
