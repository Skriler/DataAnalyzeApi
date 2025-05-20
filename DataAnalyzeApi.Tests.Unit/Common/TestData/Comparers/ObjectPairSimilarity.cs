namespace DataAnalyzeApi.Tests.Common.TestData.Comparers;

/// <summary>
/// Represents the similarity percent between two objects identified by their indices.
/// </summary>
public class ObjectPairSimilarity
{
    /// <summary>
    /// Index of the first object.
    /// </summary>
    public long ObjectAIndex { get; set; }

    /// <summary>
    /// Index of the second object.
    /// </summary>
    public long ObjectBIndex { get; set; }

    /// <summary>
    /// Similarity percentage between two objects.
    /// </summary>
    public double SimilarityPercentage { get; set; }
}
