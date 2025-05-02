using ENGHelperBot.Services.Command;
using ENGHelperBot.Services.Parsers.CallbackData;
using ENGHelperBot.Services.Repositories.Dictionaries;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ENGHelperBot.Services.Commands.Dictionaries;

public class RemoveDictionaryCommand(IServiceScopeFactory scopeFactory) : ICommandHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<Message> HandleAsync(ITelegramBotClient bot, Update update)
    {
        const string msg = $"""
            🎉 <b>Cловарь удален!</b>
        """;

        var query = update.CallbackQuery!;

        using var scope = _scopeFactory.CreateScope();

        var callbackDataParser = scope.ServiceProvider.GetRequiredService<ICallbackDataParser>();
        var dictionaryService = scope.ServiceProvider.GetRequiredService<IDictionaryRepository>();

        var parsedData = callbackDataParser.Parse(query.Data!);
        if (parsedData.SelectionData is not { } selectionData)
            throw new ArgumentException("There are no selection data for remove dictionary.");

        await dictionaryService.DeleteAsync(d => d.Id == selectionData.Id);

        return await bot.SendMessage(query.Message!.Chat, msg, ParseMode.Html);
    }
}
