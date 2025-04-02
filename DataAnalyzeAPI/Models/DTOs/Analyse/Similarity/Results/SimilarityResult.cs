using DataAnalyzeAPI.Models.Domain.Similarity;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Results;

public class SimilarityResult
{
    public long DatasetId { get; set; }

    public List<SimilarityPair> Similarities { get; set; } = new();
}
