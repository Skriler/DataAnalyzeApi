using DataAnalyzeApi.Unit.Common.Models.Analyse;

namespace DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers;

/// <summary>
/// Test case class for all clusterer tests
/// </summary>
public record BaseClustererTestCase
{
    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<NormalizedDataObject> Objects { get; init; } = new();

    /// <summary>
    /// Pairwise distances between objects, used for mocking distance calculation.
    /// </summary>
    public List<ObjectPairDistance> PairwiseDistances { get; init; } = new();

    /// <summary>
    /// Expected sizes of each cluster.
    /// </summary>
    public List<int> ExpectedClusterSizes { get; init; } = new();

    /// <summary>
    /// Expected number of clusters.
    /// </summary>
    public int ExpectedClusterCount => ExpectedClusterSizes.Count;
}
