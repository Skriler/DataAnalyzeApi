namespace DataAnalyzeApi.Models.Domain.DimensionalityReduction.Pca;

public record EigenDecomposition(
    double[,] Vectors,
    double[] Values
);
