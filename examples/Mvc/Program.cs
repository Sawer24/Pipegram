using Microsoft.Extensions.DependencyInjection;
using Mvc.Services;
using Pipegram;
using Pipegram.Controllers;
using Pipegram.Controllers.CallbackQueries;
using Pipegram.Controllers.Messages;
using Pipegram.Interceptions;
using Pipegram.Routing;
using Telegram.Bot;

var token = File.ReadAllText(File.Exists("TOKEN.txt") ? "TOKEN.txt" : "../../../../../TOKEN.txt");
var options = new TelegramBotClientOptions(token);
var builder = TelegramApplicationBuilder.CreateBuilder(options);

builder.Services.AddSingleton<IItemService, ItemService>();

builder.Services.AddMessageControllers();
builder.Services.AddCallbackQueryControllers();

var application = builder.Build();

application.UseInterceptors();
application.UseRouting();
application.UseEndpoints();

application.MapControllers();

application.Run();
