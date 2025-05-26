namespace DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Results;

public record SimilarityResult
{
    public long DatasetId { get; init; }

    public List<SimilarityPairDto> Similarities { get; init; } = new();
}
