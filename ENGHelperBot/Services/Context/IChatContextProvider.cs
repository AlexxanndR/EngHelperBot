using ENGHelperBot.Services.Command;

namespace ENGHelperBot.Services.Context;

public interface IChatContextProvider
{
    string? GetFollowingCommand(long chatId);
    void SetFollowingCommand(long chatId, string command);
    void ResetFollowingCommand(long chatId);
}
