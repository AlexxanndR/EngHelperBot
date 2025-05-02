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
            ?
            data.Select(d => new[] { InlineKeyboardButton.WithCallbackData(d.Text, $"{BotCommands.SelectDictionary};{d.Id}") })
                .Concat(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(BotCommandTexts.Back),
                        InlineKeyboardButton.WithCallbackData($"1/{totalPages}"),
                        InlineKeyboardButton.WithCallbackData(BotCommandTexts.Forward, $"{BotCommands.Next};dict;1")
                    },
                    new[] { InlineKeyboardButton.WithCallbackData(BotCommandTexts.AddDictionary, BotCommands.AddDictionaryClick) },
                })
            :
            [
                [InlineKeyboardButton.WithCallbackData(BotCommandTexts.RenameDictionary, $"{BotCommands.RenameDictionaryClick};{selectionData.Id}")],
                [InlineKeyboardButton.WithCallbackData(BotCommandTexts.RemoveDictionary, $"{BotCommands.RemoveDictionary};{selectionData.Id}")]
            ];

        return await bot.SendMessage(query.Message!.Chat.Id, msg, parseMode: ParseMode.Html, replyMarkup: reply.ToArray());
    }
}
