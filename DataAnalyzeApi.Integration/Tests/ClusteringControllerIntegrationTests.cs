using DataAnalyzeApi.Integration.Common.Assertions;
using DataAnalyzeApi.Integration.Common.Factories;
using DataAnalyzeApi.Integration.Common.TestData.Clustering;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Enums;
using System.Net;
using System.Net.Http.Json;

namespace DataAnalyzeApi.Integration.Tests;

public class ClusteringControllerIntegrationTests : IntegrationTestBase
{
    private readonly string BaseUrl = "/api/analysis/clustering";

    [Theory]
    [MemberData(nameof(ClusteringControllerTestData.ValidClusteringRequestTestCases),
        MemberType = typeof(ClusteringControllerTestData))]
    public async Task CalculateClusters_WhenValidRequest_ReturnsExpectedClusteringResult(
        ClusteringTestCase testCase)
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();

        // Act
        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}/{testCase.Method}/{datasetId}",
            testCase.Request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<ClusteringAnalysisResultDto>();
        AnalysisResultAssertions.AssertClusteringResult(
            result,
            datasetId,
            testCase.Request.IncludeParameters);
    }

    [Fact]
    public async Task CalculateKMeansClusters_WhenCachedResultExists_ReturnsExpectedClusteringResult()
    {
        // Arrange
        const bool includeParameters = true;
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = ClusteringRequestFactory.CreateKMeans(
            includeParameters: includeParameters,
            numberOfClusters: 3,
            maxIterations: 100,
            numericMetric: NumericDistanceMetricType.Cosine,
            categoricalMetric: CategoricalDistanceMetricType.Jaccard);

        // Act
        var firstResponse = await client.PostAsJsonAsync(
            $"{BaseUrl}/kmeans/{datasetId}",
            request);

        var secondResponse = await client.PostAsJsonAsync(
            $"{BaseUrl}/kmeans/{datasetId}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);

        var firstResult = await firstResponse.Content.ReadFromJsonAsync<ClusteringAnalysisResultDto>();
        AnalysisResultAssertions.AssertClusteringResult(firstResult, datasetId, includeParameters);

        var secondResult = await secondResponse.Content.ReadFromJsonAsync<ClusteringAnalysisResultDto>();
        AnalysisResultAssertions.AssertClusteringResult(secondResult, datasetId, includeParameters);
    }

    [Theory]
    [MemberData(nameof(ClusteringControllerTestData.ValidClusteringRequestTestCases),
        MemberType = typeof(ClusteringControllerTestData))]
    public async Task CalculateClusters_WithNegativeDatasetId_ReturnsBadRequest(
        ClusteringTestCase testCase)
    {
        // Arrange
        const int negativeDatasetId = -5;

        // Act
        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}/{testCase.Method}/{negativeDatasetId}",
            testCase.Request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(ClusteringControllerTestData.ValidClusteringRequestTestCases),
        MemberType = typeof(ClusteringControllerTestData))]
    public async Task CalculateClusters_WhenNonExistentDataset_ReturnsNotFound(
        ClusteringTestCase testCase)
    {
        // Arrange
        const int nonExistentDatasetId = 99999;

        // Act
        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}/{testCase.Method}/{nonExistentDatasetId}",
            testCase.Request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(ClusteringControllerTestData.InvalidClusteringRequestTestCases),
        MemberType = typeof(ClusteringControllerTestData))]
    public async Task CalculateClusters_WhenInvalidParameters_ReturnsBadRequest(
        ClusteringTestCase testCase)
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();

        // Act
        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}/{testCase.Method}/{datasetId}",
            testCase.Request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
