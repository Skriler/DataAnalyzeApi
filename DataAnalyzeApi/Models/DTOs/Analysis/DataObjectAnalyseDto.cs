using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.DTOs.Analysis;

public record DataObjectAnalysisDto
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string> ParameterValues { get; set; } = default!;

    public DataObjectAnalysisDto() { }

    public DataObjectAnalysisDto(
        long id,
        string name,
        Dictionary<string, string> parameterValues = null!)
    {
        Id = id;
        Name = name;
        ParameterValues = parameterValues;
    }
}
