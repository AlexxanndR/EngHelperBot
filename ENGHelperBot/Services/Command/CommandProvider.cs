using ENGHelperBot.Services.Context;

namespace ENGHelperBot.Services.Command;

public class CommandProvider(IChatsContext chatsContext) : ICommandProvider
{
    private readonly IChatsContext _chatsContext = chatsContext;

    public ICommandHandler Get(long chatId) 
        => _chatsContext.GetFollowingCommand(chatId) ?? new StartCommand();
}
