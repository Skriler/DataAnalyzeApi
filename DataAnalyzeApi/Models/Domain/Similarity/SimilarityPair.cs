using DataAnalyzeApi.Models.Domain.Dataset;

namespace DataAnalyzeApi.Models.Domain.Similarity;

public record SimilarityPair(
    DataObjectModel ObjectA,
    DataObjectModel ObjectB,
    double SimilarityPercentage
);
