using DataAnalyzeApi.Integration.Common.Factories;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Integration.Common.TestData.Clustering;

/// <summary>
/// Class with test data for AuthControllerIntegrationTests.
/// </summary>
public static class ClusteringControllerTestData
{
    public static TheoryData<ClusteringTestCase> ValidClusteringRequestTestCases =>
    [
        // Test Case 1: kmeans test case
        new ClusteringTestCase
        {
            Method = "kmeans",

            Request = ClusteringRequestFactory.CreateKMeans(
                includeParameters: false,
                numberOfClusters: 3,
                maxIterations: 100),
        },

        // Test Case 2: dbscan test case
        new ClusteringTestCase
        {
            Method = "dbscan",

            Request = ClusteringRequestFactory.CreateDBSCAN(
                includeParameters: true,
                epsilon: 0.2,
                minPoints: 5,
                numericMetric: NumericDistanceMetricType.Manhattan,
                parameterSettings: []),
        },

        // Test Case 3: agglomerative test case
        new ClusteringTestCase
        {
            Method = "agglomerative",

            Request = ClusteringRequestFactory.CreateAgglomerative(
                includeParameters: false,
                threshold: 0.25,
                categoricalMetric: CategoricalDistanceMetricType.Jaccard),
        },
    ];

    public static TheoryData<ClusteringTestCase> InvalidClusteringRequestTestCases =>
    [
        // Test Case 1: invalid kmeans request
        new ClusteringTestCase
        {
            Method = "kmeans",

            Request = ClusteringRequestFactory.CreateKMeans(
                includeParameters: false,
                numberOfClusters: -1, // Invalid cluster count
                maxIterations: 2000, // Invalid iteration count
                categoricalMetric: CategoricalDistanceMetricType.Hamming,
                parameterSettings:
                [
                    new() { ParameterId = 1, Weight = -1.0 }, // Invalid weight
                ]),
        },

        // Test Case 2: invalid dbscan request
        new ClusteringTestCase
        {
            Method = "dbscan",

            Request = ClusteringRequestFactory.CreateDBSCAN(
                includeParameters: true,
                epsilon: -0.5, // Invalid epsilon
                minPoints: 200, // Invalid min points
                numericMetric: NumericDistanceMetricType.Euclidean,
                parameterSettings:
                [
                    new() { ParameterId = 1, Weight = 10.5 }, // Invalid weight
                ]),
        },

        // Test Case 3: invalid agglomerative request
        new ClusteringTestCase
        {
            Method = "agglomerative",

            Request = ClusteringRequestFactory.CreateAgglomerative(
                includeParameters: false,
                threshold: 1.5, // Invalid threshold
                categoricalMetric: CategoricalDistanceMetricType.Hamming,
                parameterSettings:
                [
                    new() { ParameterId = 1, Weight = -1.0 }, // Invalid weight
                ]),
        },
    ];
}
