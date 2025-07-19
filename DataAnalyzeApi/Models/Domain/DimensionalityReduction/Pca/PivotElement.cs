namespace DataAnalyzeApi.Models.Domain.DimensionalityReduction.Pca;

public record PivotElement(
    int Row,
    int Column,
    double Value
);
