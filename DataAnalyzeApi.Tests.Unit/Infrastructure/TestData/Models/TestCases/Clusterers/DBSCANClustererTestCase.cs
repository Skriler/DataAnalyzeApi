namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases.Clusterers;

/// <summary>
/// Test case class for DBSCANClustererTests
/// </summary>
public class DBSCANClustererTestCase : BaseClustererTestCase
{
    /// <summary>
    /// The epsilon for clustering.
    /// </summary>
    public double Epsilon { get; set; }

    /// <summary>
    /// The min points for clustering.
    /// </summary>
    public int MinPoints { get; set; }

    /// <summary>
    /// Indicates whether a noise cluster is expected.
    /// </summary>
    public bool ExpectNoiseCluster { get; set; }
}
