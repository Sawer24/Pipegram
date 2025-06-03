# Pipegram
ASP-style framework for creating telegram bots.

---

## Simplest bot example
```csharp
var options = new TelegramBotClientOptions("TOKEN");
var builder = TelegramBotBuilder.CreateBuilder(options);

builder.Services.AddMessageRouting();

var bot = builder.Build();

bot.UseRouting();
bot.UseEndpoints();

bot.MapMessage("/start", () => Results.SendMessage("Hello, World!"));

await bot.RunAsync();
```

---

## Message controllers
```csharp
var options = new TelegramBotClientOptions("TOKEN");
var builder = TelegramBotBuilder.CreateBuilder(options);

builder.Services.AddMessageRouting();

var bot = builder.Build();

bot.UseMessageInterceptor();
bot.UseRouting();
bot.UseEndpoints();

bot.MapMessageController<MyMessageController>();

await bot.RunAsync();

class MyMessageController : MessageControllerBase
{
    [MessageAction("/start")]
    public async Task Start()
    {
        await SendMessage("Hello, World!");
    }
}
```

---

## Registration forms are now even easier
```csharp
class LoginController : MessageControllerBase
{
    [MessageAction("/start")]
    public async Task Start()
    {
        await SendMessage("Please sent your firstname:");
        var firstname = (await InterceptMessage())!.Message!.Text;
        await SendMessage("Please sent your lastname:");
        var lastname = (await InterceptMessage())!.Message!.Text;
        await SendMessage("Please sent your email address:");
        var email = (await InterceptMessage())!.Message!.Text;
        await SendMessage($"Your details:\nFirstname: {firstname}\nLastname: {lastname}\nEmail: {email}");
    }
}
```

---

## Inline keyboard manipulations example
```csharp
var options = new TelegramBotClientOptions("TOKEN");
var builder = TelegramBotBuilder.CreateBuilder(options);

builder.Services.AddMessageRouting();
builder.Services.AddCallbackQueryRouting();

var bot = builder.Build();

bot.UseMessageInterceptor();
bot.UseRouting();
bot.UseEndpoints();

bot.MapCallbackQueryController<MenuController>();
bot.MapMessage("/start", () => Results.SendMessage("Menu", replyMarkup: MenuController.mainMenu));

await bot.RunAsync();

class MenuController : CallbackQueryControllerBase
{
    public static InlineKeyboardMarkup mainMenu = new([
        [new InlineKeyboardButton("Items", "items")],
        [new InlineKeyboardButton("Profile", "profile"),
        new InlineKeyboardButton("About", "about")]]);

    [CallbackQueryAction("start")]
    public async Task Start() => await EditMessage("Menu", replyMarkup: mainMenu);

    [CallbackQueryAction("items")]
    public async Task Items() => await EditMessage("Items list", replyMarkup: InlineKeyboardMarkup([
            [CallbackButton("Item 1", "item", "item_1")],
            [CallbackButton("Item 2", "item", "item_2")],
            [CallbackButton("Back", "start")]]));

    [CallbackQueryAction("item")]
    public async Task Item(string itemId) => await EditMessage($"Details for {itemId}",
        replyMarkup: InlineKeyboardMarkup(CallbackButton("Back", "items")));

    [CallbackQueryAction("profile")]
    public async Task Profile() => await EditMessage("Profile details",
        replyMarkup: InlineKeyboardMarkup(CallbackButton("Back", "start")));

    [CallbackQueryAction("about")]
    public async Task About() => await EditMessage("About this bot",
        replyMarkup: InlineKeyboardMarkup(CallbackButton("Back", "start")));
}

```
