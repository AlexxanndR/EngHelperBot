using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ENGHelperBot.Services;

public class UpdateHandler(ITelegramBotClient bot) : IUpdateHandler
{
    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        //await (update switch
        //{
        //    { Message: { } message } => OnMessage(message),
        //    { EditedMessage: { } message } => OnMessage(message),
        //    { CallbackQuery: { } callbackQuery } => OnCallbackQuery(callbackQuery),
        //    { InlineQuery: { } inlineQuery } => OnInlineQuery(inlineQuery),
        //    { ChosenInlineResult: { } chosenInlineResult } => OnChosenInlineResult(chosenInlineResult),
        //    { Poll: { } poll } => OnPoll(poll),
        //    { PollAnswer: { } pollAnswer } => OnPollAnswer(pollAnswer),
        //    // ChannelPost:
        //    // EditedChannelPost:
        //    // ShippingQuery:
        //    // PreCheckoutQuery:
        //    _ => UnknownUpdateHandlerAsync(update)
        //});
    }
}
