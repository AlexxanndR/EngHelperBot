using ENGHelperBot.Data;
using Telegram.Bot.Types;

namespace ENGHelperBot.Services.Parsers.CallbackData;

public class CallbackDataParser : ICallbackDataParser
{
    public ParsedCallbackData Parse(string callbackData)
    {
        var parameters = callbackData.Split(';', StringSplitOptions.RemoveEmptyEntries);
        
        if (parameters.Length <= 1) 
            throw new ArgumentException("Invalid callback data format");
        
        var command = parameters[0];
        return command switch
        {
            BotCommands.Previous or BotCommands.Next => ParsePaginationData(callbackData[1..]),
            _ => throw new ArgumentException($"Unknown command: {command}")
        };
    }

    private ParsedCallbackData ParsePaginationData(params string[] parameters)
    {
        if (parameters.Length < 2)
            throw new ArgumentException("Invalid callback data format");

        var dataType = parameters[0] switch
        {
            "dict" => PaginationData.DataType.Dictionary,
            "word" => PaginationData.DataType.Word,
            _ => throw new ArgumentException($"Unknown data type: {parameters[0]}")
        };
        var pageNumber = int.TryParse(parameters[1], out var value) 
            ? value 
            : throw new ArgumentException($"Invalid page number: {value}");

        return new ParsedCallbackData { PaginationData = new(dataType, pageNumber) };
    }
}
