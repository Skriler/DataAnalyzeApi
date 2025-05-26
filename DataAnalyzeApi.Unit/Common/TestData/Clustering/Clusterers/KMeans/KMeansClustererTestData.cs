using DataAnalyzeApi.Unit.Common.Models.Analyse;

namespace DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers.KMeans;

public static class KMeansClustererTestData
{
    public static TheoryData<KMeansClustererTestCase> KMeansClustererTestCases() =>
    [
        // Test Case 1: 4 objects & 2 clusters, sizes - [2, 2]
        new KMeansClustererTestCase
        {
            Objects =
            [
                // Very distinct clusters, should converge quickly
                // Cluster 1
                new NormalizedDataObject
                {
                    NumericValues = [0.1, 0.1],
                    CategoricalValues = [[1, 0, 0]],
                },
                // Centroid of cluster 1
                new NormalizedDataObject
                {
                    NumericValues = [0.15, 0.15],
                    CategoricalValues = [[1, 0, 0]],
                },

                // Cluster 2
                new NormalizedDataObject
                {
                    NumericValues = [0.9, 0.9],
                    CategoricalValues = [[0, 0, 1]],
                },
                // Centroid of cluster 2
                new NormalizedDataObject
                {
                    NumericValues = [0.95, 0.95],
                    CategoricalValues = [[0, 0, 1]],
                },
            ],

            PairwiseDistances =
            [
                // Distances within Cluster 1
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.05 },

                // Distances within Cluster 2
                new() { ObjectAIndex = 2, ObjectBIndex = 3, Distance = 0.05 },

                // Distances between clusters (very large)
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.85 },
                new() { ObjectAIndex = 0, ObjectBIndex = 3, Distance = 0.9 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.8 },
                new() { ObjectAIndex = 1, ObjectBIndex = 3, Distance = 0.85 },
            ],

            MaxIterations = 10,
            NumberOfClusters = 2,

            ExpectedClusterSizes = [2, 2],
        },


        // Test Case 2: 5 objects & 2 clusters, sizes - [3, 2]
        new KMeansClustererTestCase
        {
            Objects =
            [
                // Cluster 1
                new NormalizedDataObject
                {
                    NumericValues = [0.1, 0.1],
                    CategoricalValues = [[1, 0, 0]],
                },
                // Centroid of cluster 1
                new NormalizedDataObject
                {
                    NumericValues = [0.15, 0.15],
                    CategoricalValues = [[1, 0, 0]],
                },

                // Cluster 2
                new NormalizedDataObject
                {
                    NumericValues = [0.8, 0.8],
                    CategoricalValues = [[0, 0, 1]],
                },
                // Centroid of cluster 2
                new NormalizedDataObject
                {
                    NumericValues = [0.85, 0.85],
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
                // Distances within Cluster 1
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.05 },

                // Distances within Cluster 2
                new() { ObjectAIndex = 2, ObjectBIndex = 3, Distance = 0.05 },
                new() { ObjectAIndex = 2, ObjectBIndex = 4, Distance = 0.1 },
                new() { ObjectAIndex = 3, ObjectBIndex = 4, Distance = 0.05 },

                // Distances between clusters
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.75 },
                new() { ObjectAIndex = 0, ObjectBIndex = 3, Distance = 0.8 },
                new() { ObjectAIndex = 0, ObjectBIndex = 4, Distance = 0.85 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.7 },
                new() { ObjectAIndex = 1, ObjectBIndex = 3, Distance = 0.75 },
                new() { ObjectAIndex = 1, ObjectBIndex = 4, Distance = 0.8 },
            ],

            MaxIterations = 10,
            NumberOfClusters = 2,

            ExpectedClusterSizes = [3, 2],
        },

        // Test Case 3: 5 objects & 3 clusters, sizes - [2, 2, 1]
        // Objects naturally form 2 clusters but we ask for 3, algorithm should split one of the natural clusters
        new KMeansClustererTestCase
        {
            Objects =
            [
                // Cluster 1
                // Centroid of cluster 1
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
                // Centroid of cluster 2
                new NormalizedDataObject
                {
                    NumericValues = [0.8, 0.8],
                    CategoricalValues = [[0, 0, 1]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.85, 0.85],
                    CategoricalValues = [[0, 0, 1]],
                },

                // Cluster 3
                // Centroid of cluster 3
                new NormalizedDataObject
                {
                    NumericValues = [0.9, 0.9],
                    CategoricalValues = [[0, 1, 1]],
                },
            ],

            PairwiseDistances =
            [
                // Distances within Natural Cluster 1
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.05 },

                // Distances within Natural Cluster 2
                new() { ObjectAIndex = 2, ObjectBIndex = 3, Distance = 0.05 },
                new() { ObjectAIndex = 2, ObjectBIndex = 4, Distance = 0.12 },
                new() { ObjectAIndex = 3, ObjectBIndex = 4, Distance = 0.07 },

                // Distances between clusters
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.9 },
                new() { ObjectAIndex = 0, ObjectBIndex = 3, Distance = 0.95 },
                new() { ObjectAIndex = 0, ObjectBIndex = 4, Distance = 1.0 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.85 },
                new() { ObjectAIndex = 1, ObjectBIndex = 3, Distance = 0.9 },
                new() { ObjectAIndex = 1, ObjectBIndex = 4, Distance = 0.95 },
            ],

            MaxIterations = 10,
            NumberOfClusters = 3,

            // Algorithm should split one of the natural clusters
            ExpectedClusterSizes = [2, 2, 1],
        },


        // Test Case 4: 8 objects & 3 clusters, sizes - [3, 3, 2]
        new KMeansClustererTestCase
        {
            Objects =
            [
                // Cluster 1
                new NormalizedDataObject
                {
                    NumericValues = [0.1, 0.1],
                    CategoricalValues = [[1, 0, 0]],
                },
                // Centroid of cluster 1
                new NormalizedDataObject
                {
                    NumericValues = [0.12, 0.12],
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
                // Centroid of cluster 2
                new NormalizedDataObject
                {
                    NumericValues = [0.45, 0.45],
                    CategoricalValues = [[0, 1, 0]],
                },

                // Cluster 3
                new NormalizedDataObject
                {
                    NumericValues = [0.9, 0.9],
                    CategoricalValues = [[0, 0, 1]],
                },
                // Centroid of cluster 3
                new NormalizedDataObject
                {
                    NumericValues = [0.95, 0.95],
                    CategoricalValues = [[0, 0, 1]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.85, 0.85],
                    CategoricalValues = [[0, 0, 1]],
                }
            ],

            PairwiseDistances =
            [
                // Distances within Cluster 1
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.02 },

                // Distances within Cluster 2
                new() { ObjectAIndex = 2, ObjectBIndex = 3, Distance = 0.05 },
                new() { ObjectAIndex = 2, ObjectBIndex = 4, Distance = 0.05 },
                new() { ObjectAIndex = 3, ObjectBIndex = 4, Distance = 0.1 },

                // Distances within Cluster 3
                new() { ObjectAIndex = 5, ObjectBIndex = 6, Distance = 0.05 },
                new() { ObjectAIndex = 5, ObjectBIndex = 7, Distance = 0.05 },
                new() { ObjectAIndex = 6, ObjectBIndex = 7, Distance = 0.1 },

                // Distances between Cluster 1 and Cluster 2
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.5 },
                new() { ObjectAIndex = 0, ObjectBIndex = 3, Distance = 0.55 },
                new() { ObjectAIndex = 0, ObjectBIndex = 4, Distance = 0.6 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.45 },
                new() { ObjectAIndex = 1, ObjectBIndex = 3, Distance = 0.5 },
                new() { ObjectAIndex = 1, ObjectBIndex = 4, Distance = 0.55 },

                // Distances between Cluster 1 and Cluster 3
                new() { ObjectAIndex = 0, ObjectBIndex = 5, Distance = 0.8 },
                new() { ObjectAIndex = 0, ObjectBIndex = 6, Distance = 0.85 },
                new() { ObjectAIndex = 0, ObjectBIndex = 7, Distance = 0.9 },
                new() { ObjectAIndex = 1, ObjectBIndex = 5, Distance = 0.75 },
                new() { ObjectAIndex = 1, ObjectBIndex = 6, Distance = 0.8 },
                new() { ObjectAIndex = 1, ObjectBIndex = 7, Distance = 0.85 },

                // Distances between Cluster 2 and Cluster 3
                new() { ObjectAIndex = 2, ObjectBIndex = 5, Distance = 0.55 },
                new() { ObjectAIndex = 2, ObjectBIndex = 6, Distance = 0.6 },
                new() { ObjectAIndex = 2, ObjectBIndex = 7, Distance = 0.5 },
                new() { ObjectAIndex = 3, ObjectBIndex = 5, Distance = 0.5 },
                new() { ObjectAIndex = 3, ObjectBIndex = 6, Distance = 0.55 },
                new() { ObjectAIndex = 3, ObjectBIndex = 7, Distance = 0.45 },
                new() { ObjectAIndex = 4, ObjectBIndex = 5, Distance = 0.6 },
                new() { ObjectAIndex = 4, ObjectBIndex = 6, Distance = 0.65 },
                new() { ObjectAIndex = 4, ObjectBIndex = 7, Distance = 0.55 },
            ],

            MaxIterations = 10,
            NumberOfClusters = 3,

            ExpectedClusterSizes = [3, 3, 2],
        },
    ];
};
