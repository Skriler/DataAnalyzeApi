using DataAnalyzeApi.Models.Enum;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.DTOs.Dataset.Read;

public record ParameterDto
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ParameterType Type { get; init; }
}
