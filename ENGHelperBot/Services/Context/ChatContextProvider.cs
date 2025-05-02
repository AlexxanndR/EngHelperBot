using System.Collections.Concurrent;

namespace ENGHelperBot.Services.Context;

public class ChatContextProvider : IChatContextProvider
{
    private ConcurrentDictionary<long, string> FollowingContext { get; } = new();

    public string? GetFollowingContext(long chatId)
        => FollowingContext.TryGetValue(chatId, out var command) ? command : default;

    public void SetFollowingContext(long chatId, string command)
        => FollowingContext.AddOrUpdate(chatId, command, (k, v) => v);
}
