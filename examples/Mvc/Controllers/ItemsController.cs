using Mvc.Services;
using Mvc.Views;
using Pipegram.Controllers;
using Pipegram.Routing;
using Telegram.Bot.Types.ReplyMarkups;

namespace Mvc.Controllers;

public class ItemsController(IItemService itemService) : CallbackQueryControllerBase
{
    private readonly IItemService _itemService = itemService;

    [CallbackQueryAction(Endpoints.Items.List)]
    public async Task<IResult> List() => new ItemsListView(await _itemService.GetItems());

    [CallbackQueryAction(Endpoints.Items.Item)]
    public async Task<IResult> Item(string id)
    {
        var item = await _itemService.GetItem(int.Parse(id));

        if (item is null)
            return new ItemsListView(await _itemService.GetItems());

        return new ItemsItemView(item);
    }

    [CallbackQueryAction(Endpoints.Items.Create)]
    public async Task<IResult> Create()
    {
        await EditMessage("Please send the name of the new item:", replyMarkup: new InlineKeyboardButton("Cancel", "-"));
        var name = await InterceptMessage(deleteAfterIntercept: true);
        if (!name.IsMessage)
            return new ItemsListView(await _itemService.GetItems());
        
        await EditMessage("Please send the description of the new item:",
            replyMarkup: new[] { new InlineKeyboardButton("No description", "no-desc"), new InlineKeyboardButton("Cancel", "-") });
        var description = await InterceptMessage(deleteAfterIntercept: true);
        if (!description.IsMessage && description.CallbackQuery?.Data != "no-desc")
            return new ItemsListView(await _itemService.GetItems());

        var item = await _itemService.AddItem(name.Message.Text ?? "Unnamed", description.Message?.Text);
        return new ItemsItemView(item);
    }

    [CallbackQueryAction(Endpoints.Items.Edit)]
    public async Task<IResult> Edit(string id)
    {
        var item = await _itemService.GetItem(int.Parse(id));
        if (item is null)
            return new ItemsListView(await _itemService.GetItems());

        await EditMessage($"Please send the new description for the item '{item.Name}':",
            replyMarkup: new[] { new InlineKeyboardButton("No description", "no-desc"), new InlineKeyboardButton("Cancel", "-") });
        var description = await InterceptMessage(deleteAfterIntercept: true);
        if (!description.IsMessage && description.CallbackQuery?.Data != "no-desc")
            return new ItemsItemView(item);

        item = await _itemService.UpdateItem(item.Id, item.Name, description.Message?.Text);
        return new ItemsItemView(item);
    }

    [CallbackQueryAction(Endpoints.Items.Delete)]
    public async Task<IResult> Delete(string id)
    {
        var item = await _itemService.GetItem(int.Parse(id));
        if (item is null)
            return new ItemsListView(await _itemService.GetItems());

        await EditMessage($"Are you sure you want to delete the item '{item.Name}'?",
            replyMarkup: new[] { new InlineKeyboardButton("Yes", "yes"), new InlineKeyboardButton("No", "-") });
        var confirmation = await InterceptCallbackQuery();
        if (confirmation.CallbackQuery?.Data != "yes")
            return new ItemsItemView(item);

        await _itemService.RemoveItem(item.Id);
        return new ItemsListView(await _itemService.GetItems());
    }
}
