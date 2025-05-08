using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Normalizers;

/// <summary>
/// Class with test data for DataSetNormalizerTests
/// </summary>
public static class DatasetNormalizerTestData
{
    public static IEnumerable<object[]> GetNormalizeTestCases() =>
        new List<object[]>
        {
            // Test Case 1: Numeric only
            new object[]
            {
                new NormalizerTestCase
                {
                    RawObjects = new List<RawDataObject>
                    {
                        new RawDataObject
                        {
                            Values = { "10", "40", "25", "55" }
                        },

                        new RawDataObject
                        {
                            Values = { "20", "30", "45", "50" }
                        },

                        new RawDataObject
                        {
                            Values = { "30", "20", "30", "35" }
                        },
                    },

                    NormalizedObjects = new List<NormalizedDataObject>
                    {
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.00, 1.00, 0.00, 1.00 }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.50, 0.50, 1.00, 0.75 }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 1.00, 0.00, 0.25, 0.00 }
                        },
                    },
                },
            },

            // Test Case 2: Categorical only
            new object[]
            {
                new NormalizerTestCase
                {
                    RawObjects = new List<RawDataObject>
                    {
                        new RawDataObject
                        {
                            Values = { "Red", "Circle", "Small" }
                        },

                        new RawDataObject
                        {
                            Values = { "Red", "Square", "Large" }
                        },

                        new RawDataObject
                        {
                            Values = { "Blue", "Circle", "Small" }
                        },
                    },

                    NormalizedObjects = new List<NormalizedDataObject>
                    {
                        new NormalizedDataObject
                        {
                            CategoricalValues = { new[] { 1, 0 }, new[] { 1, 0 }, new[] { 1, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            CategoricalValues = { new[] { 1, 0 }, new[] { 0, 1 }, new[] { 0, 1 } }
                        },

                        new NormalizedDataObject
                        {
                            CategoricalValues = { new[] { 0, 1 }, new[] { 1, 0 }, new[] { 1, 0 } }
                        },
                    },
                },
            },

            // Test Case 3: Mixed values
            new object[]
            {
                new NormalizerTestCase
                {
                    // Initial values
                    RawObjects = new List<RawDataObject>
                    {
                        new RawDataObject
                        {
                            Values = { "1", "40", "500", "Red", "Circle", "Run, Swim" }
                        },

                        new RawDataObject
                        {
                            Values = { "3", "10", "300", "Green", "Square", "Fly" }
                        },

                        new RawDataObject
                        {
                            Values = { "5", "30", "100", "Blue", "Circle", "Swim, Climb" }
                        },

                        new RawDataObject
                        {
                            Values = { "4", "50", "300", "Red", "Square", "Run" }
                        },

                        new RawDataObject
                        {
                            Values = { "2", "20", "400", "Blue", "Circle", "Run, Climb, Fly" }
                        },
                    },

                    // Normalized values
                    NormalizedObjects = new List<NormalizedDataObject>
                    {
                        new NormalizedDataObject
                        {
                            NumericValues = { 0.00, 0.75, 1.00 },
                            CategoricalValues = { new[] { 1, 0, 0 }, new[] { 1, 0 }, new[] { 1, 1, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.50, 0.00, 0.50 },
                            CategoricalValues = { new[] { 0, 1, 0 }, new[] { 0, 1 }, new[] { 0, 0, 1, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 1.00, 0.50, 0.00 },
                            CategoricalValues = { new[] { 0, 0, 1 }, new[] { 1, 0 }, new[] { 0, 1, 0, 1 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.75, 1.00, 0.50 },
                            CategoricalValues = { new[] { 1, 0, 0 }, new[] { 0, 1 }, new[] { 1, 0, 0, 0 } }
                        },

                        new NormalizedDataObject
                        {
                            NumericValues = { 0.25, 0.25, 0.75 },
                            CategoricalValues = { new[] { 0, 0, 1 }, new[] { 1, 0 }, new[] { 1, 0, 1, 1 } }
                        },
                    },
                },
            },
        };
}
