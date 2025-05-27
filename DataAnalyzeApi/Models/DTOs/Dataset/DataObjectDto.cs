using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.DTOs.Dataset;

public record DataObjectDto(
    long Id,
    string Name,

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    Dictionary<string, string> ParameterValues
);
