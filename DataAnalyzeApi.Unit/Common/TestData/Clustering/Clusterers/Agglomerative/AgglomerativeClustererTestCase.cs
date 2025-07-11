namespace DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers.Agglomerative;

/// <summary>
/// Test case class for AgglomerativeClustererTests
/// </summary>
public record AgglomerativeClustererTestCase : BaseClustererTestCase
{
    /// <summary>
    /// The threshold for clustering.
    /// </summary>
    public double Threshold { get; init; }
}
