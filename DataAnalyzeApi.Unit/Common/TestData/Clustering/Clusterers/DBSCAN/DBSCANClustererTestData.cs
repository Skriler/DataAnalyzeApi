using DataAnalyzeApi.Unit.Common.Models.Analyse;

namespace DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers.DBSCAN;

/// <summary>
/// Class with test data for DBSCANClustererTests
/// </summary>
public static class DBSCANClustererTestData
{
    public static TheoryData<DBSCANClustererTestCase> DBSCANClustererTestCases() =>
    [
        // Test Case 1: 3 objects with small epsilon & 0 clusters, 3 noise objects
        new DBSCANClustererTestCase
        {
            Objects =
            [
                new NormalizedDataObject
                {
                    NumericValues = [0.1, 0.2],
                    CategoricalValues = [[1, 0, 0]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.8, 0.9],
                    CategoricalValues = [[0, 1, 0]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.4, 0.5],
                    CategoricalValues = [[0, 0, 1]],
                },
            ],

            PairwiseDistances =
            [
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.7 },
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.5 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.6 },
            ],

            Epsilon = 0.2,
            MinPoints = 2,

            ExpectedClusterSizes = [3],
            ExpectNoiseCluster = true,
        },

        // Test Case 2: 5 objects with optimal epsilon, 2 minPoints & 2 clusters, 1 noise object
        new DBSCANClustererTestCase
        {
            Objects =
            [
                // Cluster 1
                new NormalizedDataObject
                {
                    NumericValues = [0.1, 0.2],
                    CategoricalValues = [[1, 0, 0]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.15, 0.25],
                    CategoricalValues = [[1, 0, 0]],
                },

                // Cluster 2
                new NormalizedDataObject
                {
                    NumericValues = [0.8, 0.9],
                    CategoricalValues = [[0, 0, 1]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.85, 0.95],
                    CategoricalValues = [[0, 0, 1]],
                },

                // Noise
                new NormalizedDataObject
                {
                    NumericValues = [0.5, 0.5],
                    CategoricalValues = [[0, 1, 0]],
                },
            ],
            PairwiseDistances =
            [
                // Distances within Cluster 1
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.1 },

                // Distances between Cluster 1 and Cluster 2
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.9 },
                new() { ObjectAIndex = 0, ObjectBIndex = 3, Distance = 0.95 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.85 },
                new() { ObjectAIndex = 1, ObjectBIndex = 3, Distance = 0.9 },

                // Distances within Cluster 2
                new() { ObjectAIndex = 2, ObjectBIndex = 3, Distance = 0.1 },

                // Distances to noise object
                new() { ObjectAIndex = 0, ObjectBIndex = 4, Distance = 0.45 },
                new() { ObjectAIndex = 1, ObjectBIndex = 4, Distance = 0.4 },
                new() { ObjectAIndex = 2, ObjectBIndex = 4, Distance = 0.45 },
                new() { ObjectAIndex = 3, ObjectBIndex = 4, Distance = 0.5 },
            ],

            Epsilon = 0.2,
            MinPoints = 2,

            ExpectedClusterSizes = [2, 2, 1],
            ExpectNoiseCluster = true,
        },

        // Test Case 3: 6 objects with large epsilon, 3 minPoints & 1 cluster, 2 noise objects
        new DBSCANClustererTestCase
        {
            Objects =
            [
                // Cluster
                new NormalizedDataObject
                {
                    NumericValues = [0.1, 0.1],
                    CategoricalValues = [[1, 0, 0]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.2, 0.2],
                    CategoricalValues = [[1, 0, 0]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.3, 0.3],
                    CategoricalValues = [[1, 0, 0]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.35, 0.35],
                    CategoricalValues = [[1, 0, 0]],
                },

                // Noise
                new NormalizedDataObject
                {
                    NumericValues = [0.8, 0.8],
                    CategoricalValues = [[0, 0, 1]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.9, 0.9],
                    CategoricalValues = [[0, 0, 1]],
                },
            ],

            PairwiseDistances =
            [
                // Distances within cluster
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.15 },
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.3 },
                new() { ObjectAIndex = 0, ObjectBIndex = 3, Distance = 0.35 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.15 },
                new() { ObjectAIndex = 1, ObjectBIndex = 3, Distance = 0.2 },
                new() { ObjectAIndex = 2, ObjectBIndex = 3, Distance = 0.1 },

                // Distances to noise objects
                new() { ObjectAIndex = 0, ObjectBIndex = 4, Distance = 0.9 },
                new() { ObjectAIndex = 0, ObjectBIndex = 5, Distance = 1.0 },
                new() { ObjectAIndex = 1, ObjectBIndex = 4, Distance = 0.85 },
                new() { ObjectAIndex = 1, ObjectBIndex = 5, Distance = 0.95 },
                new() { ObjectAIndex = 2, ObjectBIndex = 4, Distance = 0.7 },
                new() { ObjectAIndex = 2, ObjectBIndex = 5, Distance = 0.8 },
                new() { ObjectAIndex = 3, ObjectBIndex = 4, Distance = 0.6 },
                new() { ObjectAIndex = 3, ObjectBIndex = 5, Distance = 0.7 },

                // Distance between noise objects
                new() { ObjectAIndex = 4, ObjectBIndex = 5, Distance = 0.15 },
            ],

            Epsilon = 0.4,
            MinPoints = 3,

            ExpectedClusterSizes = [4, 2],
            ExpectNoiseCluster = true,
        },

        // Test Case 4: 7 objects with optimal epsilon, 2 minPoints & 3 clusters
        new DBSCANClustererTestCase
        {
            Objects =
            [
                // Cluster 1
                new NormalizedDataObject
                {
                    NumericValues = [0.1, 0.1],
                    CategoricalValues = [[1, 0, 0]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.15, 0.15],
                    CategoricalValues = [[1, 0, 0]],
                },

                // Cluster 2
                new NormalizedDataObject
                {
                    NumericValues = [0.5, 0.5],
                    CategoricalValues = [[0, 1, 0]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.55, 0.55],
                    CategoricalValues = [[0, 1, 0]],
                },

                // Cluster 3
                new NormalizedDataObject
                {
                    NumericValues = [0.9, 0.9],
                    CategoricalValues = [[0, 0, 1]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.95, 0.95],
                    CategoricalValues = [[0, 0, 1]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.85, 0.85],
                    CategoricalValues = [[0, 0, 1]],
                },
            ],

            PairwiseDistances =
            [
                // Distances within Cluster 1
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.1 },

                // Distances within Cluster 2
                new() { ObjectAIndex = 2, ObjectBIndex = 3, Distance = 0.1 },

                // Distances within Cluster 3
                new() { ObjectAIndex = 4, ObjectBIndex = 5, Distance = 0.1 },
                new() { ObjectAIndex = 4, ObjectBIndex = 6, Distance = 0.1 },
                new() { ObjectAIndex = 5, ObjectBIndex = 6, Distance = 0.15 },

                // Distances between Cluster 1 and Cluster 2
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.6 },
                new() { ObjectAIndex = 0, ObjectBIndex = 3, Distance = 0.65 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.55 },
                new() { ObjectAIndex = 1, ObjectBIndex = 3, Distance = 0.6 },

                // Distances between Cluster 1 and Cluster 3
                new() { ObjectAIndex = 0, ObjectBIndex = 4, Distance = 0.85 },
                new() { ObjectAIndex = 0, ObjectBIndex = 5, Distance = 0.9 },
                new() { ObjectAIndex = 0, ObjectBIndex = 6, Distance = 1.0 },
                new() { ObjectAIndex = 1, ObjectBIndex = 4, Distance = 0.90 },
                new() { ObjectAIndex = 1, ObjectBIndex = 5, Distance = 0.95 },
                new() { ObjectAIndex = 1, ObjectBIndex = 6, Distance = 0.90 },

                // Distances between Cluster 2 and Cluster 3
                new() { ObjectAIndex = 2, ObjectBIndex = 4, Distance = 0.6 },
                new() { ObjectAIndex = 2, ObjectBIndex = 5, Distance = 0.65 },
                new() { ObjectAIndex = 2, ObjectBIndex = 6, Distance = 0.55 },
                new() { ObjectAIndex = 3, ObjectBIndex = 4, Distance = 0.55 },
                new() { ObjectAIndex = 3, ObjectBIndex = 5, Distance = 0.6 },
                new() { ObjectAIndex = 3, ObjectBIndex = 6, Distance = 0.5 },
            ],

            Epsilon = 0.2,
            MinPoints = 2,

            ExpectedClusterSizes = [3, 2, 2],
            ExpectNoiseCluster = false,
        },

        // Test Case 5: 8 objects with optimal epsilon, 2 minPoints, expected 1 cluster
        new DBSCANClustererTestCase
        {
            Objects =
            [
                new NormalizedDataObject
                {
                    NumericValues = [0.1, 0.1],
                    CategoricalValues = [[1, 0, 0]]
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.2, 0.2],
                    CategoricalValues = [[1, 0, 0]]
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.3, 0.3],
                    CategoricalValues = [[1, 0, 0]]
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.4, 0.4],
                    CategoricalValues = [[0, 1, 0]]
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.5, 0.5],
                    CategoricalValues = [[0, 1, 0]]
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.6, 0.6],
                    CategoricalValues = [[0, 1, 0]]
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.7, 0.7],
                    CategoricalValues = [[0, 0, 1]]
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.8, 0.8],
                    CategoricalValues = [[0, 0, 1]]
                }
            ],

            PairwiseDistances =
            [
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.15 },
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.25 },
                new() { ObjectAIndex = 0, ObjectBIndex = 3, Distance = 0.35 },
                new() { ObjectAIndex = 0, ObjectBIndex = 4, Distance = 0.45 },
                new() { ObjectAIndex = 0, ObjectBIndex = 5, Distance = 0.55 },
                new() { ObjectAIndex = 0, ObjectBIndex = 6, Distance = 0.65 },
                new() { ObjectAIndex = 0, ObjectBIndex = 7, Distance = 0.75 },

                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.15 },
                new() { ObjectAIndex = 1, ObjectBIndex = 3, Distance = 0.25 },
                new() { ObjectAIndex = 1, ObjectBIndex = 4, Distance = 0.35 },
                new() { ObjectAIndex = 1, ObjectBIndex = 5, Distance = 0.45 },
                new() { ObjectAIndex = 1, ObjectBIndex = 6, Distance = 0.55 },
                new() { ObjectAIndex = 1, ObjectBIndex = 7, Distance = 0.65 },

                new() { ObjectAIndex = 2, ObjectBIndex = 3, Distance = 0.15 },
                new() { ObjectAIndex = 2, ObjectBIndex = 4, Distance = 0.25 },
                new() { ObjectAIndex = 2, ObjectBIndex = 5, Distance = 0.35 },
                new() { ObjectAIndex = 2, ObjectBIndex = 6, Distance = 0.45 },
                new() { ObjectAIndex = 2, ObjectBIndex = 7, Distance = 0.55 },

                new() { ObjectAIndex = 3, ObjectBIndex = 4, Distance = 0.15 },
                new() { ObjectAIndex = 3, ObjectBIndex = 5, Distance = 0.25 },
                new() { ObjectAIndex = 3, ObjectBIndex = 6, Distance = 0.35 },
                new() { ObjectAIndex = 3, ObjectBIndex = 7, Distance = 0.45 },

                new() { ObjectAIndex = 4, ObjectBIndex = 5, Distance = 0.15 },
                new() { ObjectAIndex = 4, ObjectBIndex = 6, Distance = 0.25 },
                new() { ObjectAIndex = 4, ObjectBIndex = 7, Distance = 0.35 },

                new() { ObjectAIndex = 5, ObjectBIndex = 6, Distance = 0.15 },
                new() { ObjectAIndex = 5, ObjectBIndex = 7, Distance = 0.25 },

                new() { ObjectAIndex = 6, ObjectBIndex = 7, Distance = 0.15 },
            ],

            Epsilon = 0.2,
            MinPoints = 2,

            ExpectedClusterSizes = [8],
            ExpectNoiseCluster = false,
        }
    ];
}
