using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases.Clusterers;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Clusterers;

/// <summary>
/// Class with test data for DBSCANClustererTests
/// </summary>
public static class DBSCANCClustererTestData
{
    public static IEnumerable<object[]> GetDBSCANClustererTestCases() =>
        new List<object[]>
        {
            // Test Case 1: 3 objects with small epsilon & 0 clusters, 3 noise objects
            new object[]
            {
                new DBSCANClustererTestCase
                {
                    Objects = new List<NormalizedDataObject>
                    {
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.1, 0.2 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.8, 0.9 },
                            CategoricalValues = { new[] { 0, 1, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.4, 0.5 },
                            CategoricalValues = { new[] { 0, 0, 1 } }
                        },
                    },

                    PairwiseDistances = new List<ObjectPairDistance>
                    {
                        new() { ObjectAIndex = 0, ObjectBIndex = 1, Distance = 0.7 },
                        new() { ObjectAIndex = 0, ObjectBIndex = 2, Distance = 0.5 },
                        new() { ObjectAIndex = 1, ObjectBIndex = 2, Distance = 0.6 },
                    },

                    Epsilon = 0.2,
                    MinPoints = 2,

                    ExpectedClusterSizes = new List<int> { 3 },
                    ExpectNoiseCluster = true,
                },
            },

            // Test Case 2: 5 objects with optimal epsilon, 2 minPoints & 2 clusters, 1 noise objects
            new object[]
            {
                new DBSCANClustererTestCase
                {
                    Objects = new List<NormalizedDataObject>
                    {
                        // Cluster 1
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.1, 0.2 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.15, 0.25 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        // Cluster 2
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.8, 0.9 },
                            CategoricalValues = { new[] { 0, 0, 1 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.85, 0.95 },
                            CategoricalValues = { new[] { 0, 0, 1 } }
                        },

                        // Noise
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.5, 0.5 },
                            CategoricalValues = { new[] { 0, 1, 0 } }
                        },
                    },

                    PairwiseDistances = new List<ObjectPairDistance>
                    {
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
                    },

                    Epsilon = 0.2,
                    MinPoints = 2,

                    ExpectedClusterSizes = new List<int> { 2, 2, 1 },
                    ExpectNoiseCluster = true
                },
            },

            // Test Case 3: 6 objects with large epsilon, 3 minPoints3 & 1 cluster, 2 noise objects
            new object[]
            {
                new DBSCANClustererTestCase
                {
                    Objects = new List<NormalizedDataObject>
                    {
                        // Cluster
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.1, 0.1 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.2, 0.2 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.3, 0.3 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.35, 0.35 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        // Noise
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.8, 0.8 },
                            CategoricalValues = { new[] { 0, 0, 1 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.9, 0.9 },
                            CategoricalValues = { new[] { 0, 0, 1 } }
                        }
                    },

                    PairwiseDistances = new List<ObjectPairDistance>
                    {
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
                    },

                    Epsilon = 0.4,
                    MinPoints = 3,

                    ExpectedClusterSizes = new List<int> { 4, 2 },
                    ExpectNoiseCluster = true
                },
            },

            // Test Case 4: 7 objects with optimal epsilon, 2 minPoints & 3 clusters
            new object[]
            {
                new DBSCANClustererTestCase
                {
                    Objects = new List<NormalizedDataObject>
                    {
                        // Cluster 1
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.1, 0.1 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.15, 0.15 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        // Cluster 2
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.5, 0.5 },
                            CategoricalValues = { new[] { 0, 1, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.55, 0.55 },
                            CategoricalValues = { new[] { 0, 1, 0 } }
                        },

                        // Cluster 3
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.9, 0.9 },
                            CategoricalValues = { new[] { 0, 0, 1 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.95, 0.95 },
                            CategoricalValues = { new[] { 0, 0, 1 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.85, 0.85 },
                            CategoricalValues = { new[] { 0, 0, 1 } }
                        }
                    },

                    PairwiseDistances = new List<ObjectPairDistance>
                    {
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
                    },

                    Epsilon = 0.2,
                    MinPoints = 2,

                    ExpectedClusterSizes = new List<int> { 3, 2, 2 },
                    ExpectNoiseCluster = false
                },
            },

            // Test Case 5: 8 objects with optimal epsilon, 2 minPoints & 1 cluster
            new object[]
            {
                new DBSCANClustererTestCase
                {
                    Objects = new List<NormalizedDataObject>
                    {
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.1, 0.1 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.2, 0.2 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.3, 0.3 },
                            CategoricalValues = { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.4, 0.4 },
                            CategoricalValues = { new[] { 0, 1, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.5, 0.5 },
                            CategoricalValues = { new[] { 0, 1, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.6, 0.6 },
                            CategoricalValues = { new[] { 0, 1, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.7, 0.7 },
                            CategoricalValues = { new[] { 0, 0, 1 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.8, 0.8 },
                            CategoricalValues = { new[] { 0, 0, 1 } }
                        }
                    },

                    PairwiseDistances = new List<ObjectPairDistance>
                    {
                        // All pairwise distances are within epsilon
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
                    },

                    Epsilon = 0.2,
                    MinPoints = 2,

                    ExpectedClusterSizes = new List<int> { 8 },
                    ExpectNoiseCluster = false
                },
            },
        };
}
