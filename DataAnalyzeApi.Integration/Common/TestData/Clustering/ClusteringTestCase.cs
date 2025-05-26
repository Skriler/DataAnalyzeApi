using DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Requests;

namespace DataAnalyzeApi.Integration.Common.TestData.Clustering;

/// <summary>
/// Test case for ClusteringControllerIntegrationTests.
/// </summary>
public record ClusteringTestCase
{
    /// <summary>
    /// The name of the clustering method ("kmeans", "dbscan", "agglomerative").
    /// </summary>
    public string Method { get; init; } = string.Empty;

    /// <summary>
    /// The clustering request object specific to the selected method.
    /// </summary>
    public BaseClusteringRequest Request { get; init; } = default!;
}
