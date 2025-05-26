namespace DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers.DBSCAN;

/// <summary>
/// Test case class for DBSCANClustererTests
/// </summary>
public record DBSCANClustererTestCase : BaseClustererTestCase
{
    /// <summary>
    /// The epsilon for clustering.
    /// </summary>
    public double Epsilon { get; init; }

    /// <summary>
    /// The min points for clustering.
    /// </summary>
    public int MinPoints { get; init; }

    /// <summary>
    /// Indicates whether a noise cluster is expected.
    /// </summary>
    public bool ExpectNoiseCluster { get; init; }
}
