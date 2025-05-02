using ENGHelperBot.Data;
using ENGHelperBot.Data.Entities;
using ENGHelperBot.Services.Parsers.CallbackData;
using ENGHelperBot.Services.Repositories.Dictionaries;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ENGHelperBot.Services.Command;

public class ForwardCommand(IServiceScopeFactory scopeFactory) : ICommandHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<Message> HandleAsync(ITelegramBotClient bot, Update update)
    {
        var query = update.CallbackQuery!;

        using var scope = _scopeFactory.CreateScope();
        var callbackDataParser = scope.ServiceProvider.GetRequiredService<ICallbackDataParser>();

        var parsedData = callbackDataParser.Parse(query.Data!);
        if (parsedData.PaginationData is not { } paginationData)
            throw new ArgumentException("There are no pagination data.");

        var (data, totalPages) = await (paginationData.Type switch
        {
            PaginationData.DataType.Dictionary => GetDictionaryPageAsync(paginationData.CurrentPage + 1)
            //PaginationData.DataType.Word => ...
        });

        var reply = new InlineKeyboardButton[][]
        {
            data.Select(d => InlineKeyboardButton.WithCallbackData(d.Name)).ToArray(),
            [InlineKeyboardButton.WithCallbackData(BotCommandTexts.AddDictionary, BotCommands.AddDictionary)],
            [
                InlineKeyboardButton.WithCallbackData(BotCommandTexts.Back, $"{BotCommands.Previous};dict;1"),
                InlineKeyboardButton.WithCallbackData($"{paginationData.CurrentPage + 1}"),
                InlineKeyboardButton.WithCallbackData(BotCommandTexts.Forward, $"{BotCommands.Next};dict;1")
            ],

        };

        var replyMarkup = query.Message!.ReplyMarkup!;
        var fullReply = reply.Concat(replyMarkup.InlineKeyboard.Skip(reply.Length)).ToArray();

        return await bot.EditMessageReplyMarkup(query.Message!.Chat, query.Message.Id, replyMarkup: fullReply);
    }

    private async Task<(IEnumerable<Dictionary> Data, int TotalPages)> GetDictionaryPageAsync(int page)
    {
        using var scope = _scopeFactory.CreateScope();
        var dictionaryService = scope.ServiceProvider.GetRequiredService<IDictionaryRepository>();
        return await dictionaryService.GetByPageAsync(pageNumber: page, pageSize: 5);
    }
}
