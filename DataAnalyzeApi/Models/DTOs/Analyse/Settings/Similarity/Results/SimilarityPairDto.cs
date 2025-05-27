using DataAnalyzeApi.Models.DTOs.Dataset;

namespace DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Results;

public record SimilarityPairDto(
    DataObjectDto ObjectA,
    DataObjectDto ObjectB,
    double SimilarityPercentage
);
