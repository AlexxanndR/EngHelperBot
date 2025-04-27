using ENGHelperBot.Services.Context;
using Telegram.Bot.Types;

namespace ENGHelperBot.Services.Command.Provider;

public class CommandProvider(IChatContextProvider chatContextProvider, IServiceScopeFactory scopeFactory) : ICommandProvider
{
    private readonly IChatContextProvider _chatContextProvider = chatContextProvider;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public ICommandHandler Get(long chatId, string command)
    {
        Func<string, ICommandHandler?> GetHandler = (string command) => command switch
        {
            BotCommands.Start => new StartCommand(_scopeFactory),
            BotCommandTexts.MyDictionaries => new MyDictionariesCommand(_scopeFactory),
            BotCommands.AddDictionary => new AddDictionaryCommand(_scopeFactory),
            BotCommands.AddDictionaryClick => new AddDictionaryClickCommand(_scopeFactory),
            BotCommands.SelectDictionary => new SelectDictionaryCommand(_scopeFactory),
            BotCommandTexts.AddWord => new AddWordCommand(),
            BotCommands.Next => new ForwardCommand(_scopeFactory),
            BotCommands.Previous => new BackCommand(_scopeFactory),
            BotCommandTexts.TakeTest => new TakeTestCommand(),
            _ => null
        };

        var handler = GetHandler(command);
    
        if (handler != null)
             return handler;

        var followingCommand = _chatContextProvider.GetFollowingCommand(chatId)
            ?? throw new ArgumentException($"Chat {chatId}: command {command} not exist.");

        handler = GetHandler(followingCommand) 
            ?? throw new ArgumentException($"Chat {chatId}: command {command} not exist.");

        return handler;
    }
}
