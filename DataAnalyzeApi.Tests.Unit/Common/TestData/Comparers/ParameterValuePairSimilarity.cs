namespace DataAnalyzeApi.Tests.Common.TestData.Comparers;

/// <summary>
/// Represents the similarity percent between two values identified by their indices and parameter index.
/// </summary>
public class ParameterValuePairSimilarity
{
    /// <summary>
    /// Actual first value.
    /// </summary>
    public string ValueA { get; set; } = string.Empty;

    /// <summary>
    /// Actual first value.
    /// </summary>
    public string ValueB { get; set; } = string.Empty;

    /// <summary>
    /// Similarity percentage between two values.
    /// </summary>
    public double SimilarityPercentage { get; set; }
}
