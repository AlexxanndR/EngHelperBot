using ENGHelperBot.Services.Command;
using System.Collections.Concurrent;

namespace ENGHelperBot.Services.Context;

public class ChatContextProvider : IChatContextProvider
{
    private ConcurrentDictionary<long, string> FollowingCommands { get; } = new();

    public string? GetFollowingCommand(long chatId)
        => FollowingCommands.TryGetValue(chatId, out var command) ? command : default;
    public void SetFollowingCommand(long chatId, string command)
        => FollowingCommands[chatId] = command;

    public void ResetFollowingCommand(long chatId)
        => FollowingCommands.Remove(chatId, out var _);
}
