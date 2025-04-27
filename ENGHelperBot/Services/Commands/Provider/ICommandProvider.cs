namespace ENGHelperBot.Services.Command.Provider;

public interface ICommandProvider
{
    ICommandHandler Get(long chatId, string command);
}
