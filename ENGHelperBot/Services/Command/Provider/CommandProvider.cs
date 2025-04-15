using ENGHelperBot.Services.Context;

namespace ENGHelperBot.Services.Command.Provider;

public class CommandProvider(IServiceProvider serviceProvider, IChatsContext chatsContext) : ICommandProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IChatsContext _chatsContext = chatsContext;

    public ICommandHandler Get(long chatId, string messageText)
        => messageText switch
        {
            BotCommands.Start => new StartCommand(_serviceProvider),
            BotCommands.MyDictionaries => new MyDictionariesCommand(),
            BotCommands.AddWord => new AddWordCommand(),
            BotCommands.TakeTest => new TakeTestCommand(),
            _ => _chatsContext.GetFollowingCommand(chatId) ?? throw new ArgumentNullException($"{chatId} — {messageText}"),
        };
}
