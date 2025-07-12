namespace DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

public record SimilarityResult
{
    public long DatasetId { get; init; }

    public List<SimilarityPairDto> Similarities { get; init; } = [];
}
