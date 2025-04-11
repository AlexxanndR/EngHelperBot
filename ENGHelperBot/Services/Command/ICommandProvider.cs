namespace ENGHelperBot.Services.Command;

public interface ICommandProvider
{
    ICommandHandler Get(long chatId);
}
