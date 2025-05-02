namespace ENGHelperBot.Data;

public record ParsedCallbackData
{
    public PaginationData? PaginationData { get; set; }
    public SelectionData? SelectionData { get; set; }
}

public record PaginationData(PaginationData.DataType Type, int CurrentPage, int MessageId)
{
    public enum DataType { Dictionary, Word };
}

public record SelectionData(long Id);