using ENGHelperBot.Data;

namespace ENGHelperBot.Services.Parsers.CallbackData;

public interface ICallbackDataParser
{
    ParsedCallbackData Parse(string callbackData);
}
