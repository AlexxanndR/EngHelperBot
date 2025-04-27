using ENGHelperBot.Data;
using ENGHelperBot.Data.Entities;
using ENGHelperBot.Services.Parsers.CallbackData;
using ENGHelperBot.Services.Repositories.Dictionaries;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ENGHelperBot.Services.Command;

public class BackCommand(IServiceScopeFactory scopeFactory) : ICommandHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<Message> HandleAsync(ITelegramBotClient bot, Update update)
    {
        using var scope = _scopeFactory.CreateScope();
        var callbackDataParser = scope.ServiceProvider.GetRequiredService<ICallbackDataParser>();

        var parsedData = callbackDataParser.Parse(update.CallbackQuery!.Data!);
        if (parsedData.PaginationData is not { } paginationData)
            throw new ArgumentException("There are no pagination data.");

        var (data, totalPages) = await (paginationData.Type switch
        {
            PaginationData.DataType.Dictionary => GetDictionaryPageAsync(paginationData.CurrentPage - 1)
            //PaginationData.DataType.Word => ...
        });

        var reply = new InlineKeyboardButton[][]
        {
            data.Select(d => InlineKeyboardButton.WithCallbackData(d.Name)).ToArray(),
            [InlineKeyboardButton.WithCallbackData(BotCommandTexts.AddDictionary, BotCommands.AddDictionary)],
            [
                InlineKeyboardButton.WithCallbackData(BotCommandTexts.Back, $"{BotCommands.Previous};dict;1;{update.Message!.Id}"),
                InlineKeyboardButton.WithCallbackData($"{paginationData.CurrentPage - 1}/{totalPages}"),
                InlineKeyboardButton.WithCallbackData(BotCommandTexts.Forward, $"{BotCommands.Next};dict;1;{update.Message!.Id}")
            ],
            [InlineKeyboardButton.WithCallbackData(BotCommandTexts.AddDictionary, BotCommands.AddDictionary)]
        };

        return await bot.EditMessageReplyMarkup(update.CallbackQuery.Message!.Chat.Id, paginationData.MessageId, replyMarkup: reply);
    }

    private async Task<(IEnumerable<Dictionary> Data, int TotalPages)> GetDictionaryPageAsync(int page)
    {
        using var scope = _scopeFactory.CreateScope();
        var dictionaryService = scope.ServiceProvider.GetRequiredService<IDictionaryRepository>();
        return await dictionaryService.GetByPageAsync(pageNumber: page, pageSize: 5);
    }
}
