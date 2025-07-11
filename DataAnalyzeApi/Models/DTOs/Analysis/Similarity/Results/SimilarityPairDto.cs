namespace DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

public record SimilarityPairDtoDto(
    DataObjectAnalysisDto ObjectA,
    DataObjectAnalysisDto ObjectB,
    double SimilarityPercentage
);
