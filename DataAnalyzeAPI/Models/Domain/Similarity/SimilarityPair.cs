using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Models.Domain.Similarity;

public record SimilarityPair(
    DataObjectModel ObjectA,
    DataObjectModel ObjectB,
    double SimilarityPercentage
);
