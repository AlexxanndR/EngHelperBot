using Telegram.Bot;
using Telegram.Bot.Types;

namespace ENGHelperBot.Services.Command;

public interface ICommandHandler
{
    Task<Message> HandleAsync(ITelegramBotClient bot, Update update); 
}
