namespace DataAnalyzeApi.Models.DTOs.Dataset.Read;

public record DatasetDto
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public List<ParameterDto> Parameters { get; init; } = [];

    public List<DataObjectDto> Objects { get; init; } = [];
}
