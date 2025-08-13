namespace DataAnalyzeApi.Models.DTOs.Common;

public record PaginationResult<T>
{
    public List<T> Data { get; init; } = [];

    public int TotalCount { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public bool HasNext => PageNumber * PageSize < TotalCount;

    public bool HasPrevious => PageNumber > 1;

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
