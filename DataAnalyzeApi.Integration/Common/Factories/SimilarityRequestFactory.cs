using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Requests;

namespace DataAnalyzeApi.Integration.Common.Factories;

/// <summary>
/// Factory for creating test instances of SimilarityRequest.
/// </summary>
public static class SimilarityRequestFactory
{
    /// <summary>
    /// Creates a SimilarityRequest with the specified parameters or default ones.
    /// </summary>
    public static SimilarityRequest Create(
        bool includeParameters,
        List<ParameterSettingsDto>? parameterSettings = null)
    {
        return new SimilarityRequest
        {
            ParameterSettings = parameterSettings ?? GetDefaultParameterSettings(),
            IncludeParameters = includeParameters
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
            new() { ParameterId = 2, IsActive = true, Weight = 1.5 },
        ];
    }
}
