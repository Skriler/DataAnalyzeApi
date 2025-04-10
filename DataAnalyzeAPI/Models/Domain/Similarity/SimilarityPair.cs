using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeAPI.Models.Domain.Similarity;

public record SimilarityPair(
    DataObjectModel ObjectA,
    DataObjectModel ObjectB,
    double SimilarityPercentage
);
