using DataAnalyzeAPI.Models.DTOs.Dataset;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Results;

public record SimilarityPairDto(
    DataObjectDto ObjectA,
    DataObjectDto ObjectB,
    double SimilarityPercentage
);
