namespace ENGHelperBot.Services.Context;

public interface IChatContextProvider
{
    string? GetFollowingContext(long chatId);
    void SetFollowingContext(long chatId, string command);
    void ResetFollowingContext(long chatId);
}
