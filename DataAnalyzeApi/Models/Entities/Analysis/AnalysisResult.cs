using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities.Analysis;

public abstract class AnalysisResult
{
    [Key]
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string RequestHash { get; set; } = string.Empty;

    public bool IncludeParameters { get; set; }

    [JsonIgnore] public long DatasetId { get; set; }
    [JsonIgnore] public Dataset Dataset { get; set; } = default!;
}
