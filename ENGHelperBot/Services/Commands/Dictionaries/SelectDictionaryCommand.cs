using ENGHelperBot.Services.Command;
using ENGHelperBot.Services.Parsers.CallbackData;
using ENGHelperBot.Services.Repositories.Words;
using System.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ENGHelperBot.Services.Commands.Dictionaries;

public class SelectDictionaryCommand(IServiceScopeFactory scopeFactory) : ICommandHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<Message> HandleAsync(ITelegramBotClient bot, Update update)
    {
        var query = update.CallbackQuery!;

        using var scope = _scopeFactory.CreateScope();

        var callbackDataParser = scope.ServiceProvider.GetRequiredService<ICallbackDataParser>();
        var wordRepository = scope.ServiceProvider.GetRequiredService<IWordRepository>();

        var parsedData = callbackDataParser.Parse(query.Data!);
        if (parsedData.SelectionData is not { } selectionData)
            throw new ArgumentException("There are no selection data.");

        const string dictionaryViewMsg = $"""  
            🌟 <i>Список ваших слов (нажмите на любое для подробностей):</i>   
        """;

        const string noWordsMsg = """
            😔 <b>Ой, тут пока пусто!</b>  
        """;

        var (data, totalPages) = await wordRepository.GetByPageAsync(pageNumber: 1, pageSize: 5);
        var isWordsExist = totalPages > 0;

        var msg = isWordsExist ? dictionaryViewMsg : noWordsMsg;
        var reply = isWordsExist
            ? new InlineKeyboardButton[][]
            {
                data.Select(w => InlineKeyboardButton.WithCallbackData(w.Text, $"{BotCommands.SelectWord};{w.Id}")).ToArray(),
                [
                    InlineKeyboardButton.WithCallbackData(BotCommandTexts.Back),
                    InlineKeyboardButton.WithCallbackData($"1/{totalPages}"),
                    InlineKeyboardButton.WithCallbackData(BotCommandTexts.Forward, $"{BotCommands.Next};word;1")
                ],
                [InlineKeyboardButton.WithCallbackData(BotCommandTexts.RenameDictionary,  $"{BotCommands.RenameDictionaryClick};{selectionData.Id}")],
                [InlineKeyboardButton.WithCallbackData(BotCommandTexts.RemoveDictionary, $"{BotCommands.RemoveDictionary};{selectionData.Id}")]
            }
            :
            [
                [InlineKeyboardButton.WithCallbackData(BotCommandTexts.RenameDictionary, $"{BotCommands.RenameDictionaryClick};{selectionData.Id}")],
                [InlineKeyboardButton.WithCallbackData(BotCommandTexts.RemoveDictionary, $"{BotCommands.RemoveDictionary};{selectionData.Id}")]
            ];

        return await bot.SendMessage(query.Message!.Chat.Id, msg, parseMode: ParseMode.Html, replyMarkup: reply);
    }
}
