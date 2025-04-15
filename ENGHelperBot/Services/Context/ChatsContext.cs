using ENGHelperBot.Services.Command;
using System.Collections.Concurrent;

namespace ENGHelperBot.Services.Context;

public class ChatsContext : IChatsContext
{
    private ConcurrentDictionary<long, ICommandHandler> FollowingCommands { get; } = new();

    public ICommandHandler? GetFollowingCommand(long chatId)
        => FollowingCommands.TryGetValue(chatId, out var command) ? command : default;
    public void SetFollowingCommand(long chatId, ICommandHandler command)
        => FollowingCommands[chatId] = command;
}
