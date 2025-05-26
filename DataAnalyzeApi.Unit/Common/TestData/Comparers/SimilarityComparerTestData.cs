using DataAnalyzeApi.Unit.Common.Models.Analyse;

namespace DataAnalyzeApi.Unit.Common.TestData.Comparers;

/// <summary>
/// Class with test data for SimilarityComparerTests
/// </summary>
public static class SimilarityComparerTestData
{
    public static TheoryData<SimilarityComparerTestCase> CompareAllObjectsTestCases() =>
    [
        // Test Case 1: Numeric parameters only
        new SimilarityComparerTestCase
        {
            Objects =
            [
                new RawDataObject
                {
                    Values = ["0.2", "0.2"],
                },
                new RawDataObject
                {
                    Values = ["0.5", "0.6"],
                },
                new RawDataObject
                {
                    Values = ["0.8", "0.8"],
                },
            ],

            PairwiseSimilarities =
            [
                // Similarities between object 0 and 1
                new() { ValueA = "0.2", ValueB = "0.5", SimilarityPercentage = 0.7 },
                new() { ValueA = "0.2", ValueB = "0.6", SimilarityPercentage = 0.6 },

                // Similarities between object 0 and 2
                new() { ValueA = "0.2", ValueB = "0.8", SimilarityPercentage = 0.4 },
                new() { ValueA = "0.2", ValueB = "0.8", SimilarityPercentage = 0.4 },

                // Similarities between object 1 and 2
                new() { ValueA = "0.5", ValueB = "0.8", SimilarityPercentage = 0.7 },
                new() { ValueA = "0.6", ValueB = "0.8", SimilarityPercentage = 0.8 },
            ],

            ExpectedSimilarities =
            [
                // (0.7 + 0.6) / 2
                new() { ObjectAIndex = 0, ObjectBIndex = 1, SimilarityPercentage = 0.65 },

                // (0.4 + 0.4) / 2
                new() { ObjectAIndex = 0, ObjectBIndex = 2, SimilarityPercentage = 0.40 },

                // (0.7 + 0.8) / 2
                new() { ObjectAIndex = 1, ObjectBIndex = 2, SimilarityPercentage = 0.75 },
            ],
        },

        // Test Case 2: Categorical parameters only
        new SimilarityComparerTestCase
        {
            Objects =
            [
                new RawDataObject
                {
                    Values = ["Red", "Large"],
                },
                new RawDataObject
                {
                    Values = ["Green, Red", "Large"],
                },
                new RawDataObject
                {
                    Values = ["Green", "Small"],
                },
            ],

            PairwiseSimilarities =
            [
                // Similarities between object 0 and 1
                new() { ValueA = "Red", ValueB = "Green, Red", SimilarityPercentage = 0.5 },
                new() { ValueA = "Large", ValueB = "Large", SimilarityPercentage = 1.0 },

                // Similarities between object 0 and 2
                new() { ValueA = "Red", ValueB = "Green", SimilarityPercentage = 0.0 },
                new() { ValueA = "Large", ValueB = "Small", SimilarityPercentage = 0.0 },

                // Similarities between object 1 and 2
                new() { ValueA = "Green, Red", ValueB = "Green", SimilarityPercentage = 0.5 },
                new() { ValueA = "Large", ValueB = "Small", SimilarityPercentage = 0.0 },
            ],

            ExpectedSimilarities =
            [
                // (0.5 + 1.0) / 2
                new() { ObjectAIndex = 0, ObjectBIndex = 1, SimilarityPercentage = 0.75 },

                // (0.0 + 0.0) / 2
                new() { ObjectAIndex = 0, ObjectBIndex = 2, SimilarityPercentage = 0.00 },

                // (0.5 + 0.0) / 2
                new() { ObjectAIndex = 1, ObjectBIndex = 2, SimilarityPercentage = 0.25 },
            ],
        },

        // Test Case 3: Mixed parameters (numeric and categorical)
        new SimilarityComparerTestCase
        {
            Objects =
            [
                new RawDataObject
                {
                    Values = ["0.5", "Red, Blue", "10.0"],
                },
                new RawDataObject
                {
                    Values = ["0.6", "Blue", "12.0"],
                },
                new RawDataObject
                {
                    Values =["1.0", "Red, Green", "20.0"],
                },
            ],

            PairwiseSimilarities =
            [
                // Similarities between object 0 and 1
                new() { ValueA = "0.5", ValueB = "0.6", SimilarityPercentage = 0.9 },
                new() { ValueA = "Red, Blue", ValueB = "Blue", SimilarityPercentage = 0.4 },
                new() { ValueA = "10.0", ValueB = "12.0", SimilarityPercentage = 0.8 },

                // Similarities between object 0 and 2
                new() { ValueA = "0.5", ValueB = "1.0", SimilarityPercentage = 0.5 },
                new() { ValueA = "Red, Blue", ValueB = "Red, Green", SimilarityPercentage = 0.2 },
                new() { ValueA = "10.0", ValueB = "20.0", SimilarityPercentage = 0.5 },

                // Similarities between object 1 and 2
                new() { ValueA = "0.6", ValueB = "1.0", SimilarityPercentage = 0.6 },
                new() { ValueA = "Blue", ValueB = "Red, Green", SimilarityPercentage = 0.0 },
                new() { ValueA = "12.0", ValueB = "20.0", SimilarityPercentage = 0.6 },
            ],

            ExpectedSimilarities =
            [
                // (0.9 + 0.4 + 0.8) / 3
                new() { ObjectAIndex = 0, ObjectBIndex = 1, SimilarityPercentage = 0.70 },

                // (0.4 + 0.3 + 0.5) / 3
                new() { ObjectAIndex = 0, ObjectBIndex = 2, SimilarityPercentage = 0.40 },

                // (0.7 + 0.0 + 0.6) / 3
                new() { ObjectAIndex = 1, ObjectBIndex = 2, SimilarityPercentage = 0.40 },
            ],
        },
    ];
}
