using ENGHelperBot.Data;

namespace ENGHelperBot.Services.Parsers.CallbackData;

public class CallbackDataParser : ICallbackDataParser
{
    public ParsedCallbackData Parse(string callbackData)
    {
        var parameters = callbackData.Split(';', StringSplitOptions.RemoveEmptyEntries);
        
        if (parameters.Length <= 1) 
            throw new ArgumentException("Invalid callback data format.");
        
        var command = parameters[0];
        return command switch
        {
            BotCommands.Previous or BotCommands.Next => ParsePaginationData(parameters[1..]),
            BotCommands.SelectDictionary or BotCommands.RemoveDictionary 
            or BotCommands.RenameDictionary or BotCommands.RenameDictionaryClick => ParseSelectionData(parameters[1..]),
            _ => throw new ArgumentException($"Unknown command: {command}.")
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
        if (!int.TryParse(parameters[1], out var pageNumber)) 
            throw new ArgumentException($"Invalid page number: {pageNumber}");

        return new ParsedCallbackData { PaginationData = new(dataType, pageNumber) };
    }

    private ParsedCallbackData ParseSelectionData(params string[] parameters)
    {
        if (parameters.Length < 1)
            throw new ArgumentException("Invalid callback data format");

        if (!int.TryParse(parameters[0], out var id))
            throw new ArgumentException($"Invalid page number: {id}");

        return new ParsedCallbackData { SelectionData = new(id) };
    }
}
