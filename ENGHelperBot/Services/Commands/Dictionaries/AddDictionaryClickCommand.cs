using ENGHelperBot.Services.Command;
using ENGHelperBot.Services.Context;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ENGHelperBot.Services.Commands.Dictionaries;

public class AddDictionaryClickCommand(IServiceScopeFactory scopeFactory) : ICommandHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<Message> HandleAsync(ITelegramBotClient bot, Update update)
    {
        const string message = """
            🎨 <b>Дайте название вашему новому словарю:</b>
        """;

        using var scope = _scopeFactory.CreateScope();
        var chatContext = scope.ServiceProvider.GetRequiredService<IChatContextProvider>();
        chatContext.SetFollowingCommand(update.CallbackQuery!.Message!.Chat.Id, BotCommands.AddDictionary);

        return await bot.SendMessage(update.CallbackQuery!.Message.Chat, message, parseMode: ParseMode.Html);
    }
}
