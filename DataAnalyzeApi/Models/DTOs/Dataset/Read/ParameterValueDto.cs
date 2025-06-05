namespace DataAnalyzeApi.Models.DTOs.Dataset.Read;

public record ParameterValueDto
{
    public long ParameterId { get; init; }

    public string Value { get; init; } = string.Empty;
}
