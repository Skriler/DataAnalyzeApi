namespace DataAnalyzeApi.Tests.Common.TestData.Clustering.Clusterers.KMeans;

/// <summary>
/// Test case class for KMeanslustererTests
/// </summary>
public class KMeansClustererTestCase : BaseClustererTestCase
{
    /// <summary>
    /// The maximum number of iterations.
    /// </summary>
    public int MaxIterations { get; set; }

    /// <summary>
    /// The number of clusters.
    /// </summary>
    public int NumberOfClusters { get; set; }
}
