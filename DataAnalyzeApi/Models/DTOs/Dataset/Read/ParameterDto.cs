using System.Text.Json.Serialization;
using DataAnalyzeApi.Models.Enum;

namespace DataAnalyzeApi.Models.DTOs.Dataset.Read;

public record ParameterDto
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ParameterType Type { get; init; }
}
