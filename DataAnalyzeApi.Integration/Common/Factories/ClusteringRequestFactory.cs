using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Requests;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Integration.Common.Factories;

/// <summary>
/// Factory for creating clustering request objects for tests.
/// </summary>
public static class ClusteringRequestFactory
{
    /// <summary>
    /// Creates a KMeans clustering request with the given configuration.
    /// </summary>
    public static KMeansClusterRequest CreateKMeans(
        bool includeParameters,
        int numberOfClusters,
        int maxIterations,
        NumericDistanceMetricType numericMetric = NumericDistanceMetricType.Euclidean,
        CategoricalDistanceMetricType categoricalMetric = CategoricalDistanceMetricType.Hamming,
        List<ParameterSettingsDto>? parameterSettings = null)
    {
        return new KMeansClusterRequest
        {
            NumericMetric = numericMetric,
            CategoricalMetric = categoricalMetric,
            IncludeParameters = includeParameters,
            MaxIterations = maxIterations,
            NumberOfClusters = numberOfClusters,
            ParameterSettings = parameterSettings ?? GetDefaultParameterSettings(),
        };
    }

    /// <summary>
    /// Creates a DBSCAN clustering request with the given configuration.
    /// </summary>
    public static DBSCANClusterRequest CreateDBSCAN(
        bool includeParameters,
        double epsilon,
        int minPoints,
        NumericDistanceMetricType numericMetric = NumericDistanceMetricType.Euclidean,
        CategoricalDistanceMetricType categoricalMetric = CategoricalDistanceMetricType.Hamming,
        List<ParameterSettingsDto>? parameterSettings = null)
    {
        return new DBSCANClusterRequest
        {
            NumericMetric = numericMetric,
            CategoricalMetric = categoricalMetric,
            IncludeParameters = includeParameters,
            Epsilon = epsilon,
            MinPoints = minPoints,
            ParameterSettings = parameterSettings ?? GetDefaultParameterSettings(),
        };
    }

    /// <summary>
    /// Creates an Agglomerative clustering request with the given configuration.
    /// </summary>
    public static AgglomerativeClusterRequest CreateAgglomerative(
        bool includeParameters,
        double threshold,
        NumericDistanceMetricType numericMetric = NumericDistanceMetricType.Euclidean,
        CategoricalDistanceMetricType categoricalMetric = CategoricalDistanceMetricType.Hamming,
        List<ParameterSettingsDto>? parameterSettings = null)
    {
        return new AgglomerativeClusterRequest
        {
            NumericMetric = numericMetric,
            CategoricalMetric = categoricalMetric,
            IncludeParameters = includeParameters,
            Threshold = threshold,
            ParameterSettings = parameterSettings ?? GetDefaultParameterSettings(),
        };
    }

    /// <summary>
    /// Returns the default set of parameter settings used for test cases.
    /// </summary>
    private static List<ParameterSettingsDto> GetDefaultParameterSettings()
    {
        return
        [
            new() { ParameterId = 1, IsActive = true, Weight = 1.0 },
            new() { ParameterId = 2, IsActive = false },
        ];
    }
}
