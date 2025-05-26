using DataAnalyzeApi.Integration.Common.Assertions;
using DataAnalyzeApi.Integration.Common.Factories;
using DataAnalyzeApi.Integration.Common.TestData.Similarity;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Requests;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Results;
using System.Net;
using System.Net.Http.Json;

namespace DataAnalyzeApi.Integration.Tests;

public class SimilarityControllerIntegrationTests : IntegrationTestBase
{
    private readonly string BaseUrl = "/api/analyse/similarity";

    [Theory]
    [MemberData(nameof(SimilarityControllerTestData.ValidSimilarityRequestTestCases),
        MemberType = typeof(SimilarityControllerTestData))]
    public async Task CalculateSimilarity_WhenValidRequest_ReturnsExpectedResult(SimilarityRequest? request)
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();

        // Act
        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}/{datasetId}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<SimilarityResult>();
        AnalysisResultAssertions.AssertSimilarityResult(
            result,
            datasetId,
            request?.IncludeParameters ?? false);
    }

    [Fact]
    public async Task CalculateSimilarity_WhenCachedResultExists_ReturnsExpectedSimilarityResult()
    {
        // Arrange
        const bool includeParameters = false;

        var datasetId = await CreateDatasetFromJsonAsync();
        var request = SimilarityRequestFactory.Create(includeParameters);

        var firstResponse = await client.PostAsJsonAsync(
            $"{BaseUrl}/{datasetId}",
            request);
        
        var secondResponse = await client.PostAsJsonAsync(
            $"{BaseUrl}/{datasetId}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);

        var firstResult = await firstResponse.Content.ReadFromJsonAsync<SimilarityResult>();
        AnalysisResultAssertions.AssertSimilarityResult(firstResult, datasetId, includeParameters);

        var secondResult = await secondResponse.Content.ReadFromJsonAsync<SimilarityResult>();
        AnalysisResultAssertions.AssertSimilarityResult(secondResult, datasetId, includeParameters);
    }

    [Fact]
    public async Task CalculateSimilarity_WhenNegativeDatasetId_ReturnsBadRequest()
    {
        // Arrange
        const int negativeDatasetId = -5;
        var request = SimilarityRequestFactory.Create(includeParameters: false);

        // Act
        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}/{negativeDatasetId}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CalculateSimilarity_WhenNonExistentDataset_ReturnsNotFound()
    {
        // Arrange
        const int nonExistentDatasetId = 99999;
        var request = SimilarityRequestFactory.Create(includeParameters: false);

        // Act
        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}/{nonExistentDatasetId}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(SimilarityControllerTestData.InvalidSimilarityRequestsTestCases),
        MemberType = typeof(SimilarityControllerTestData))]
    public async Task CalculateSimilarity_WhenInvalidRequest_ReturnsBadRequest(SimilarityRequest request)
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();

        // Act
        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}/{datasetId}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
