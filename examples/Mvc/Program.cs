using Microsoft.Extensions.DependencyInjection;
using Mvc.Services;
using Mvc.Views;
using Pipegram;
using Pipegram.Controllers;
using Pipegram.Controllers.CallbackQueries;
using Pipegram.Interceptions;
using Pipegram.Routing;
using Pipegram.Routing.Messages;
using Telegram.Bot;

var token = File.ReadAllText(File.Exists("TOKEN.txt") ? "TOKEN.txt" : "../../../../../TOKEN.txt");
var options = new TelegramBotClientOptions(token);
var builder = TelegramApplicationBuilder.CreateBuilder(options);

builder.Services.AddSingleton<IItemService, ItemService>();

builder.Services.AddMessageRouting();
builder.Services.AddCallbackQueryControllers();

var application = builder.Build();

application.UseInterceptors();
application.UseRouting();
application.UseEndpoints();

application.MapMessage("/start", () => new HomeMenuView());
application.MapControllers();

application.Run();
