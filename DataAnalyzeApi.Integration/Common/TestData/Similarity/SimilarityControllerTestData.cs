using DataAnalyzeApi.Integration.Common.Factories;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Requests;

namespace DataAnalyzeApi.Integration.Common.TestData.Similarity;

/// <summary>
/// Class with test data for SimilarityControllerIntegrationTests.
/// </summary>
public static class SimilarityControllerTestData
{
    public static TheoryData<SimilarityRequest?> ValidSimilarityRequestTestCases =>
    [
        // Test Case 1: null request test
        null!,

        // Test Case 2
        SimilarityRequestFactory.Create(
            includeParameters: true,
            parameterSettings: []),

        // Test Case 3
        SimilarityRequestFactory.Create(
            includeParameters: false,
            parameterSettings:
            [
                new() { ParameterId = 2, IsActive = true, Weight = 1.5 }
            ]),

        // Test Case 4
        SimilarityRequestFactory.Create(
            includeParameters: true,
            parameterSettings:
            [
                new() { ParameterId = 1, IsActive = true, Weight = 2.0 },
                new() { ParameterId = 2, IsActive = false, Weight = 0.5 },
                new() { ParameterId = 3, IsActive = true, Weight = 1.5 },
                new() { ParameterId = 4, IsActive = true, Weight = 0.8 }
            ]),
    ];

    public static TheoryData<SimilarityRequest> InvalidSimilarityRequestsTestCases =>
    [
        // Test Case 1: duplicate parameter IDs
        SimilarityRequestFactory.Create(
            includeParameters: false,
            parameterSettings:
            [
                new() { ParameterId = 1, IsActive = true, Weight = 1.0 },
                new() { ParameterId = 1, IsActive = false, Weight = 0.5 }, // Duplicate ID
                new() { ParameterId = 2, IsActive = true, Weight = 0.8 }
            ]),

        // Test Case 2: invalid parameter weights
        SimilarityRequestFactory.Create(
            includeParameters: false,
            parameterSettings:
            [
                new() { ParameterId = 1, IsActive = true, Weight = -1.0 }, // Invalid weight (below minimum)
                new() { ParameterId = 2, IsActive = true, Weight = 10.5 }  // Invalid weight (above maximum)
            ]),
    ];
}
