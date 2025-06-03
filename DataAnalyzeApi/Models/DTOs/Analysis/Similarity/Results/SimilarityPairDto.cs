namespace DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

public record SimilarityPairDto(
    DataObjectAnalysisDto ObjectA,
    DataObjectAnalysisDto ObjectB,
    double SimilarityPercentage
);
