namespace ENGHelperBot.Data;

public record ParsedCallbackData
{
    public PaginationData? PaginationData { get; set; }
}

public record PaginationData(PaginationData.DataType Type, int CurrentPage)
{
    public enum DataType { Dictionary, Word };
}
