using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.DTOs.Analysis;

public record DataObjectAnalysisDto(
    long Id,
    string Name,

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    Dictionary<string, string> ParameterValues
);
