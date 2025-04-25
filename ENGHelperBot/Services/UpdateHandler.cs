using ENGHelperBot.Services.Command.Provider;
using ENGHelperBot.Services.Context;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ENGHelperBot.Services;

public class UpdateHandler(ICommandProvider commandProvider) : IUpdateHandler
{
    private readonly ICommandProvider _commandProvider = commandProvider;

    public async Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await (update switch
        {
            { Message: { } } => OnMessage(bot, update),
            { CallbackQuery: { } } => OnCallbackQuery(bot, update),
            _ => UnknownUpdateHandlerAsync(update)
        });
    }

    private async Task OnMessage(ITelegramBotClient bot, Update update)
    {
        var message = update.Message!;

        if (string.IsNullOrWhiteSpace(message.Text))
            return;

        var command = _commandProvider.Get(message.Chat.Id, message.Text);
        var sentMessage = await command.HandleAsync(bot, update);
        
        // TODO: add logging
    }

    private async Task OnCallbackQuery(ITelegramBotClient bot, Update update)
    {
        var callbackQuery = update.CallbackQuery!;

        if (string.IsNullOrWhiteSpace(callbackQuery.Data))
            return;

        var command = _commandProvider.Get(callbackQuery.Message!.Chat.Id, callbackQuery.Data!.Split(';')[0]);
        var sentMessage = await command.HandleAsync(bot, update);

        // TODO: add logging
    }

    private Task UnknownUpdateHandlerAsync(Update update)
        => Task.CompletedTask;
}
