namespace DataAnalyzeAPI.Models.Domain.Similarity;

public class SimilarityPair
{
    public long ObjectAId { get; set; }
    public string ObjectAName { get; set; } = string.Empty;

    public long ObjectBId { get; set; }
    public string ObjectBName { get; set; } = string.Empty;

    public double SimilarityPercentage { get; set; }
}
