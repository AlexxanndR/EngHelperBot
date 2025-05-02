using ENGHelperBot.Services.Command;
using ENGHelperBot.Services.Context;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using ENGHelperBot.Services.Parsers.CallbackData;

namespace ENGHelperBot.Services.Commands.Dictionaries;

public class RenameDictionaryClickCommand(IServiceScopeFactory scopeFactory) : ICommandHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<Message> HandleAsync(ITelegramBotClient bot, Update update)
    {
        const string message = """
            🎨 <b>Дайте новое название вашему словарю:</b>
        """;

        var query = update.CallbackQuery!;

        using var scope = _scopeFactory.CreateScope();

        var chatContext = scope.ServiceProvider.GetRequiredService<IChatContextProvider>();
        var callbackDataParser = scope.ServiceProvider.GetRequiredService<ICallbackDataParser>();

        var parsedData = callbackDataParser.Parse(query.Data!);
        if (parsedData.SelectionData is not { } selectionData)
            throw new ArgumentNullException("There are no selection data for rename dictionary.");

        chatContext.SetFollowingContext(query.Message!.Chat.Id, $"{BotCommands.RenameDictionary};{selectionData.Id}");

        return await bot.SendMessage(query.Message.Chat, message, parseMode: ParseMode.Html);
    }
}
