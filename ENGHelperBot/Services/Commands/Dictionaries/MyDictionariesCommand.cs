using ENGHelperBot.Services.Command;
using ENGHelperBot.Services.Context;
using ENGHelperBot.Services.Parsers.CallbackData;
using ENGHelperBot.Services.Repositories.Dictionaries;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ENGHelperBot.Services.Commands.Dictionaries;

public class MyDictionariesCommand(IServiceScopeFactory scopeFactory) : ICommandHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<Message> HandleAsync(ITelegramBotClient bot, Update update)
    {
        const string dictionariesMsg = """
            <b>📖 Выберите словарь или создайте новый:</b>
        """;

        const string emptyDictionariesMsg = """
            😔 <b>Ой, тут пока пусто!</b>
            Но это легко исправить — давайте создадим ваш первый словарь!  
            Просто нажмите "Добавить словарь" ниже.  
        """;

        var message = update.Message!;

        using var scope = _scopeFactory.CreateScope();
        var dictionaryService = scope.ServiceProvider.GetRequiredService<IDictionaryRepository>();

        var (data, totalPages) = await dictionaryService.GetByPageAsync(pageNumber: 1, pageSize: 5);
        var isDictionariesExist = data.Any();

        var msg = isDictionariesExist ? dictionariesMsg : emptyDictionariesMsg;
        var reply = isDictionariesExist
            ? new InlineKeyboardButton[][]
            {
                data.Select(d => InlineKeyboardButton.WithCallbackData(d.Name, BotCommands.SelectDictionary)).ToArray(),
                [
                    InlineKeyboardButton.WithCallbackData(BotCommandTexts.Back),
                    InlineKeyboardButton.WithCallbackData($"1/{totalPages}"),
                    InlineKeyboardButton.WithCallbackData(BotCommandTexts.Forward, $"{BotCommands.Next};dict;1")
                ],
                [InlineKeyboardButton.WithCallbackData(BotCommandTexts.AddDictionary, BotCommands.AddDictionaryClick)],
            }
            :
            [
                [InlineKeyboardButton.WithCallbackData(BotCommandTexts.AddDictionary, BotCommands.AddDictionaryClick)],
            ];

        return await bot.SendMessage(message.Chat, msg, parseMode: ParseMode.Html, replyMarkup: reply);
    }
}
