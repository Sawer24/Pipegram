using Mvc.Models;
using Pipegram.Routing;

namespace Mvc.Views;

public class ItemsItemView : ViewBase
{
    public ItemsItemView(Item item)
    {
        text.AppendLine($"Item: {item.Name}");
        if (item.Description is null)
            text.AppendLine("No description.");
        else
            text.AppendLine($"Description: {item.Description}");

        keyboard.AddNewRow(("Edit description", Endpoints.Items.Edit, [item.Id]));
        keyboard.AddNewRow(("Delete item", Endpoints.Items.Delete, [item.Id]));
        keyboard.AddNewRow(("Back to items list", Endpoints.Items.List));
    }
}
