using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Helpers;

/// <summary>
/// Class with test data for CentroidCalculatorTests
/// </summary>
public static class CentroidCalculatorTestData
{
    public static IEnumerable<object[]> GetRecalculateTestCases() =>
        new List<object[]>
        {
            // Test Case 1: 2 muneric
            new object[]
            {
                new CentroidTestCase
                {
                    InitialCentroid = new NormalizedDataObject
                    {
                        NumericValues = new List<double> { 0.4, 0.6 },
                    },

                    Objects = new List<NormalizedDataObject>
                    {
                        new NormalizedDataObject
                        {
                            NumericValues = new List<double> { 0.5, 0.7 },
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = new List<double> { 0.3, 0.5 },
                        }
                    },

                    ExpectedCentroid = new NormalizedDataObject
                    {
                        // Expected average:
                        // (0.4 + 0.5 + 0.3) / 3 = 0.4
                        // (0.6 + 0.7 + 0.5) / 3 = 0.6
                        NumericValues = new List<double> { 0.4, 0.6 },
                    },
                },
            },

            // Test Case 2: 1 categorical
            new object[]
            {
                new CentroidTestCase
                {
                    InitialCentroid = new NormalizedDataObject
                    {
                        CategoricalValues = new List<int[]> { new[] { 1, 0, 0 } }
                    },

                    Objects = new List<NormalizedDataObject>
                    {
                        new NormalizedDataObject
                        {
                            CategoricalValues = new List<int[]> { new[] { 0, 1, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            CategoricalValues = new List<int[]> { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            CategoricalValues = new List<int[]> { new[] { 0, 0, 1 } }
                        },
                    },

                    ExpectedCentroid = new NormalizedDataObject
                    {
                        // Expected average:
                        // [2 / 4 = 0.5, 1 / 4 = 0.25, 1 / 4 = 0.25]
                        // 0.5 >= 0.5 → 1
                        CategoricalValues = new List<int[]> { new[] { 1, 0, 0 } }
                    },
                }
            },

            // Test Case 3: 2 muneric & 2 categorical
            new object[]
            {
                new CentroidTestCase
                {
                    InitialCentroid = new NormalizedDataObject
                    {
                        NumericValues = new List<double> { 0.3, 0.3 },
                        CategoricalValues = new List<int[]> { new[] { 1, 0 }, new[] { 0, 1 } }
                    },

                    Objects = new List<NormalizedDataObject>
                    {
                        new NormalizedDataObject
                        {
                            NumericValues = new List<double> { 0.5, 0.5 },
                            CategoricalValues = new List<int[]> { new[] { 1, 0 }, new[] { 1, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = new List<double> { 0.7, 0.7 },
                            CategoricalValues = new List<int[]> { new[] { 0, 1 }, new[] { 0, 1 } }
                        },
                    },

                    ExpectedCentroid = new NormalizedDataObject
                    {
                        // Expected average numerics: (0.3 + 0.5 + 0.7) / 3 = 0.5
                        NumericValues = new List<double> { 0.5, 0.5 },

                        // Expected average categoricals:
                        // [ (1+1+0)/3 = 0.67 → 1, (0+0+1)/3 = 0.33 → 0 ] → [1, 0]
                        // [ (0+1+0)/3 = 0.33 → 0, (1+0+1)/3 = 0.67 → 1 ] → [0, 1]
                        CategoricalValues = new List<int[]> { new[] { 1, 0 }, new[] { 0, 1 } }
                    },
                }
            },

            // Test Case 4: 2 muneric & 1 categorical
            new object[]
            {
                new CentroidTestCase
                {
                    InitialCentroid = new NormalizedDataObject
                    {
                        NumericValues = new List<double> { 0.4, 0.1 },
                        CategoricalValues = new List<int[]> { new[] { 0, 1, 0 } }
                    },

                    Objects = new List<NormalizedDataObject>
                    {
                        new NormalizedDataObject
                        {
                            NumericValues = new List<double> { 0.6, 0.5 },
                            CategoricalValues = new List<int[]> { new[] { 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = new List<double> { 0.8, 0.9 },
                            CategoricalValues = new List<int[]> { new[] { 0, 1, 0 } }
                        },
                    },

                    ExpectedCentroid = new NormalizedDataObject
                    {
                        // Expected average numerics:
                        // [ (0.4+0.6+0.8)/3 = 0.6, (0.1+0.5+0.9)/3 = 0.5 ]
                        NumericValues = new List<double> { 0.6, 0.5 },

                        // Expected average categoricals:
                        // [ (0+1+0)/3 = 0.33 → 0, (1+0+1)/3 = 0.67 → 1, (0+0+0)/3 = 0 → 0 ]
                        CategoricalValues = new List<int[]> { new[] { 0, 1, 0 } }
                    },
                }
            },

            // Test Case 5: 3 muneric & 3 categorical
            new object[]
            {
                new CentroidTestCase
                {
                    InitialCentroid = new NormalizedDataObject
                    {
                        NumericValues = new List<double> { 0.2, 0.4, 0.6 },
                        CategoricalValues = new List<int[]> { new[] { 1, 0 }, new[] { 0, 1 }, new[] { 0, 0, 1 } }
                    },

                    Objects = new List<NormalizedDataObject>
                    {
                        new NormalizedDataObject
                        {
                            NumericValues = new List<double> { 0.6, 0.0, 0.7 },
                            CategoricalValues = new List<int[]> { new[] { 1, 0 }, new[] { 1, 0 }, new[] { 0, 1, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = new List<double> { 0.4, 0.2, 0.8 },
                            CategoricalValues = new List<int[]> { new[] { 0, 1 }, new[] { 0, 1 }, new[] { 1, 0, 0 } }
                        },
                    },

                    ExpectedCentroid = new NormalizedDataObject
                    {
                        // Expected average numerics:
                        // [ (0.2+0.6+0.4)/3 = 0.4, (0.4+0.0+0.2)/3 = 0.2, (0.6+0.7+0.8)/3 = 0.7 ]
                        NumericValues = new List<double> { 0.4, 0.2, 0.7 },

                        // Expected average categoricals:
                        // [ (1+1+0)/3 = 0.67 → 1, (0+0+1)/3 = 0.33 → 0 ] → [1, 0]
                        // [ (0+1+0)/3 = 0.33 → 0, (1+0+1)/3 = 0.67 → 1 ] → [0, 1]
                        // [ (0+0+1)/3 = 0.33 → 0, (0+1+0)/3 = 0.33 → 0, (1+0+0)/3 = 0.33 → 0 ] → [0, 0, 0]
                        CategoricalValues = new List<int[]> { new[] { 1, 0 }, new[] { 0, 1 }, new[] { 0, 0, 0 } }
                    },
                },
            },
        };
}
