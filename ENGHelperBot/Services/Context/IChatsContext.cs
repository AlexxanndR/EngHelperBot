using ENGHelperBot.Services.Command;

namespace ENGHelperBot.Services.Context;

public interface IChatsContext
{
    ICommandHandler? GetFollowingCommand(long chatId);
    void SetFollowingCommand(long chatId, ICommandHandler command);
}
