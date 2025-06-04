using Telegram.Bot;
using Pipegram;
using Pipegram.Interceptions;
using Pipegram.Routing;
using Pipegram.Controllers;
using Mvc.Controllers;
using Mvc.Views;
using Microsoft.Extensions.DependencyInjection;
using Mvc.Services;

var token = File.ReadAllText(File.Exists("TOKEN.txt") ? "TOKEN.txt" : "../../../../../TOKEN.txt");
var options = new TelegramBotClientOptions(token);
var builder = TelegramBotBuilder.CreateBuilder(options);

builder.Services.AddSingleton<IItemService, ItemService>();

builder.Services.AddMessageRouting();
builder.Services.AddCallbackQueryRouting();

var bot = builder.Build();

bot.UseInterceptors();
bot.UseRouting();
bot.UseEndpoints();

bot.MapMessage("/start", () => new HomeMenuView());
bot.MapCallbackQueryController<HomeController>();
bot.MapCallbackQueryController<ItemsController>();

await bot.RunAsync();
