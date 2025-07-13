namespace DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

public record SimilarityPairDto
{
    public DataObjectAnalysisDto ObjectA { get; set; } = default!;

    public DataObjectAnalysisDto ObjectB { get; set; } = default!;

    public double SimilarityPercentage { get; set; }

    public SimilarityPairDto() { }

    public SimilarityPairDto(
        DataObjectAnalysisDto objectA,
        DataObjectAnalysisDto objectB,
        double similarityPercentage)
    {
        ObjectA = objectA;
        ObjectB = objectB;
        SimilarityPercentage = similarityPercentage;
    }
}
