namespace DataAnalyzeApi.Models.DTOs.Dataset.Read;

public record DataObjectDto
{
    public string Name { get; init; } = string.Empty;

    public List<ParameterValueDto> Values { get; init; } = [];
}
