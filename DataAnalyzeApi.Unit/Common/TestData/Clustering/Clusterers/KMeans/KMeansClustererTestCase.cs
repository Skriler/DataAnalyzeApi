namespace DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers.KMeans;

/// <summary>
/// Test case class for KMeanslustererTests
/// </summary>
public record KMeansClustererTestCase : BaseClustererTestCase
{
    /// <summary>
    /// The maximum number of iterations.
    /// </summary>
    public int MaxIterations { get; init; }

    /// <summary>
    /// The number of clusters.
    /// </summary>
    public int NumberOfClusters { get; init; }
}
