namespace DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Results;

public record SimilarityResult
{
    public long DatasetId { get; set; }

    public List<SimilarityPairDto> Similarities { get; set; } = new();
}
