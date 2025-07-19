using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities.Analysis.Similarity;

public class SimilarityPair
{
    [Key]
    public long Id { get; set; }

    public long ObjectAId { get; set; }
    [JsonIgnore] public DataObject ObjectA { get; set; } = default!;

    public long ObjectBId { get; set; }
    [JsonIgnore] public DataObject ObjectB { get; set; } = default!;

    public double SimilarityPercentage { get; set; }

    [JsonIgnore] public long SimilarityAnalysisResultId { get; set; }
    [JsonIgnore] public SimilarityAnalysisResult SimilarityAnalysisResult { get; set; } = default!;
}
