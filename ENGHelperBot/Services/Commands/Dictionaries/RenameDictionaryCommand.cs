using ENGHelperBot.Services.Command;
using ENGHelperBot.Services.Context;
using ENGHelperBot.Services.Repositories.Dictionaries;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using ENGHelperBot.Services.Parsers.CallbackData;

namespace ENGHelperBot.Services.Commands.Dictionaries;

public class RenameDictionaryCommand(IServiceScopeFactory scopeFactory) : ICommandHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<Message> HandleAsync(ITelegramBotClient bot, Update update)
    {
        const string dictRenamedMsg = """
            🎉 <b>Словарь переименован!</b>
        """;

        const string dictExistsMsg = """
            ⚠️ <b>Словарь с таким названием уже существует!</b>
        """;

        var message = update.Message!;

        using var scope = _scopeFactory.CreateScope();

        var chatContext = scope.ServiceProvider.GetRequiredService<IChatContextProvider>();
        var callbackDataParser = scope.ServiceProvider.GetRequiredService<ICallbackDataParser>();
        var dictionaryService = scope.ServiceProvider.GetRequiredService<IDictionaryRepository>();

        var context = chatContext.GetFollowingContext(message.Chat.Id);

        var parsedData = callbackDataParser.Parse(context!);
        if (parsedData.SelectionData is not { } selectionData)
            throw new ArgumentNullException("There are no selection data for rename dictionary.");

        if (await dictionaryService.AnyAsync(d => d.Name == message.Text) == true)
            return await bot.SendMessage(message.Chat, dictExistsMsg, parseMode: ParseMode.Html);

        var dictionary = await dictionaryService.FindAsync(d => d.Id == selectionData.Id)
            ?? throw new ArgumentNullException($"Couldn't find dictionary with id: {selectionData.Id}");

        dictionary.Name = message.Text!;
        await dictionaryService.UpdateAsync(dictionary);

        return await bot.SendMessage(message.Chat, dictRenamedMsg, parseMode: ParseMode.Html);
    }
}
