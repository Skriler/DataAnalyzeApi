namespace DataAnalyzeAPI.Models.DTOs.Analyse.Similarity;

public class SimilarityResult
{
    public long DatasetId { get; set; }

    public List<SimilarityPairDto> Similarities { get; set; } = new();
}
