using Telegram.Bot.Types.ReplyMarkups;

namespace Pipegram.Routing;

public static class InlineKeyboardMarkupExtensions
{
    public static InlineKeyboardMarkup AddButton(this InlineKeyboardMarkup keyboard,
        (string text, string callbackEndpoint, object[] args) button)
        => keyboard.AddButton(ToButton(button));

    public static InlineKeyboardMarkup AddButtons(this InlineKeyboardMarkup keyboard,
        params IEnumerable<(string text, string callbackEndpoint, object[] args)> buttons)
        => keyboard.AddButtons([.. buttons.Select(ToButton)]);

    public static InlineKeyboardMarkup AddNewRow(this InlineKeyboardMarkup keyboard,
        params IEnumerable<(string text, string callbackEndpoint, object[] args)> buttons)
        => keyboard.AddNewRow([.. buttons.Select(ToButton)]);

    private static InlineKeyboardButton ToButton((string text, string callbackEndpoint, object[] args) button)
        => InlineKeyboardButton.WithCallbackData(
            button.text,
            button.callbackEndpoint + (button.args.Length > 0 ? ' ' + string.Join(' ', button.args) : "")
        );
}
