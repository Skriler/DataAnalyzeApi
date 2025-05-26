namespace DataAnalyzeApi.Unit.Common.TestData.Comparers;

/// <summary>
/// Represents the similarity percent between two values identified by their indices and parameter index.
/// </summary>
public record ParameterValuePairSimilarity
{
    /// <summary>
    /// Actual first value.
    /// </summary>
    public string ValueA { get; init; } = string.Empty;

    /// <summary>
    /// Actual first value.
    /// </summary>
    public string ValueB { get; init; } = string.Empty;

    /// <summary>
    /// Similarity percentage between two values.
    /// </summary>
    public double SimilarityPercentage { get; init; }
}
