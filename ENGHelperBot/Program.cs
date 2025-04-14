using ENGHelperBot;
using ENGHelperBot.Extensions;
using ENGHelperBot.Services;
using ENGHelperBot.Services.Command;
using ENGHelperBot.Services.Context;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBotConfiguration();
builder.Services.AddHttpClient("tgwebhook")
    .RemoveAllLoggers()
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) => new TelegramBotClient(sp.GetRequiredService<BotConfiguration>().BotToken, httpClient));
builder.Services.AddSingleton<UpdateHandler>();
builder.Services.AddSingleton<IChatsContext, ChatsContext>();
builder.Services.AddSingleton<ICommandProvider, CommandProvider>();
builder.Services.ConfigureDatabaseConnection();
builder.Services.ConfigureTelegramBotMvc();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MigrateAsync();
app.UseAuthorization();
app.MapControllers();

app.Run();
