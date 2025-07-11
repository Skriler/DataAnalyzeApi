using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Entities.Analysis;

public abstract class BaseAnalysis
{
    [Key]
    public long Id { get; set; }

    public long DatasetId { get; set; }
    [JsonIgnore] public Dataset Dataset { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public string RequestHash { get; set; } = string.Empty;

    public string ParameterSettingsJson { get; set; } = string.Empty;

    public bool IncludeParameters { get; set; }

    public abstract AnalysisType AnalysisType { get; }
}
