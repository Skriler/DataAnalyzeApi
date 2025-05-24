using DataAnalyzeApi.Tests.Common.Models.Analyse;

namespace DataAnalyzeApi.Tests.Common.TestData.Clustering.Clusterers.Agglomerative;

/// <summary>
/// Class with test data for AgglomerativeClustererTests
/// </summary>
public static class AgglomerativeClustererTestData
{
    public static TheoryData<AgglomerativeClustererTestCase> AgglomerativeClustererTestCases() =>
    [
        // Test Case 1: 3 objects with low threshold & 3 clusters
        new AgglomerativeClustererTestCase
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
                }
            ],

            PairwiseDistances =
            [
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.7 },
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.5 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.6 },
            ],

            Threshold = 0.1,
            ExpectedClusterSizes = [1, 1, 1],
        },

        // Test Case 2: 3 objects with optimal threshold & 2 clusters
        new AgglomerativeClustererTestCase
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
                    CategoricalValues = [[0, 1, 1]],
                }
            ],

            PairwiseDistances =
            [
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.1 },
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.7 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.6 },
            ],

            Threshold = 0.2,
            ExpectedClusterSizes = [2, 1],
        },

        // Test Case 3: 3 objects with high threshold & 1 cluster
        new AgglomerativeClustererTestCase
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
                    NumericValues = [0.15, 0.25],
                    CategoricalValues = [[1, 0, 0]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.2, 0.3],
                    CategoricalValues = [[1, 0, 0]],
                }
            ],

            PairwiseDistances =
            [
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.1 },
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.3 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.25 },
            ],

            Threshold = 0.4,
            ExpectedClusterSizes = [3],
        },

        // Test Case 4: 5 objects with optimal threshold & 3 clusters
        new AgglomerativeClustererTestCase
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
                    NumericValues = [0.2, 0.2],
                    CategoricalValues = [[1, 0, 0]],
                },

                // Cluster 2
                new NormalizedDataObject
                {
                    NumericValues = [0.5, 0.5],
                    CategoricalValues = [[0, 1, 0]],
                },

                // Cluster 3
                new NormalizedDataObject
                {
                    NumericValues = [0.8, 0.8],
                    CategoricalValues = [[0, 0, 1]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.9, 0.9],
                    CategoricalValues = [[0, 0, 1]],
                }
            ],

            PairwiseDistances =
            [
                // Distances within Cluster 1 (objects 0 and 1)
                new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.15 },

                // Distances between Cluster 1 and object 2
                new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.45 },
                new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.4 },

                // Distances between Cluster 1 and Cluster 3
                new() { ObjectAIndex = 0, ObjectBIndex = 3, Distance = 0.8 },
                new() { ObjectAIndex = 0, ObjectBIndex = 4, Distance = 0.85 },
                new() { ObjectAIndex = 1, ObjectBIndex = 3, Distance = 0.7 },
                new() { ObjectAIndex = 1, ObjectBIndex = 4, Distance = 0.75 },

                // Distances between object 2 and Cluster 3
                new() { ObjectAIndex = 2, ObjectBIndex = 3, Distance = 0.5 },
                new() { ObjectAIndex = 2, ObjectBIndex = 4, Distance = 0.55 },

                // Distances within Cluster 3 (objects 3 and 4)
                new() { ObjectAIndex = 3, ObjectBIndex = 4, Distance = 0.15 },
            ],

            Threshold = 0.2,
            ExpectedClusterSizes = [2, 1, 2],
        },
    ];
}
