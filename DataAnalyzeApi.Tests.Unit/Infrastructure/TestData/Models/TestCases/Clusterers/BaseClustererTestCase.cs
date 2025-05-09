using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases.Clusterers;

/// <summary>
/// Test case class for all clusterer tests
/// </summary>
public class BaseClustererTestCase
{
    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<NormalizedDataObject> Objects { get; set; } = new();

    /// <summary>
    /// Pairwise distances between object pairs, used for mocking distance calculation.
    /// </summary>
    public List<ObjectPairDistance>? PairwiseDistances { get; set; }

    /// <summary>
    /// Expected sizes of each cluster.
    /// </summary>
    public List<int> ExpectedClusterSizes { get; set; } = new();

    /// <summary>
    /// Expected number of clusters.
    /// </summary>
    public int ExpectedClusterCount => ExpectedClusterSizes.Count;
}
