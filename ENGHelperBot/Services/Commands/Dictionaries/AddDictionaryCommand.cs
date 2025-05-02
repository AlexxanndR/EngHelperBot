using ENGHelperBot.Services.Command;
using ENGHelperBot.Services.Context;
using ENGHelperBot.Services.Repositories.Dictionaries;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ENGHelperBot.Services.Commands.Dictionaries;

public class AddDictionaryCommand(IServiceScopeFactory scopeFactory) : ICommandHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<Message> HandleAsync(ITelegramBotClient bot, Update update)
    {
        const string dictCreatedMsg = """
            🎉 <b>Новый словарь создан!</b>
        """;

        const string dictExistsMsg = """
            ⚠️ <b>Такой словарь уже есть!</b>
        """;

        var message = update.Message!;

        using var scope = _scopeFactory.CreateScope();

        var chatContext = scope.ServiceProvider.GetRequiredService<IChatContextProvider>();
        chatContext.ResetFollowingContext(message.Chat.Id);

        var dictionaryService = scope.ServiceProvider.GetRequiredService<IDictionaryRepository>();
        var isSuccess = await dictionaryService.CreateAsync(new() { Name = message.Text!, UserId = message.Chat.Id },
                                                            d => d.UserId == message.Chat.Id);

        return await bot.SendMessage(message.Chat, isSuccess ? dictCreatedMsg : dictExistsMsg, parseMode: ParseMode.Html);
    }
}
