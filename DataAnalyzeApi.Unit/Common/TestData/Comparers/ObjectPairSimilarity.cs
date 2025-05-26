namespace DataAnalyzeApi.Unit.Common.TestData.Comparers;

/// <summary>
/// Represents the similarity percent between two objects identified by their indices.
/// </summary>
public record ObjectPairSimilarity
{
    /// <summary>
    /// Index of the first object.
    /// </summary>
    public long ObjectAIndex { get; init; }

    /// <summary>
    /// Index of the second object.
    /// </summary>
    public long ObjectBIndex { get; init; }

    /// <summary>
    /// Similarity percentage between two objects.
    /// </summary>
    public double SimilarityPercentage { get; init; }
}
