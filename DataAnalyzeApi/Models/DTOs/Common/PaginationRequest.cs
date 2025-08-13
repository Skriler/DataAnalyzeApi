namespace DataAnalyzeApi.Models.DTOs.Common;

public record PaginationRequest
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public DateTime? FromDate { get; init; }

    public DateTime? ToDate { get; init; }
}
