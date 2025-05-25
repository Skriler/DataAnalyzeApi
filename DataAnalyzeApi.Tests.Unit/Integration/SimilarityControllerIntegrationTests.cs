using DataAnalyzeApi.Models.DTOs.Analyse.Settings;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Requests;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Results;
using System.Net;
using System.Net.Http.Json;

namespace DataAnalyzeApi.Tests.Integration;

public class SimilarityControllerIntegrationTests : IntegrationTestBase
{
    private readonly string BaseUrl = "/api/analyse/similarity";

    [Fact]
    public async Task CalculateSimilarity_WhenValidDatasetAndRequest_ReturnsSuccess()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = CreateSimilarityRequest(true);

        // Act
        var response = await client.PostAsJsonAsync($"{BaseUrl}/{datasetId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SimilarityResult>();
        Assert.NotNull(result);
        Assert.Equal(datasetId, result.DatasetId);
        Assert.NotEmpty(result.Similarities);
    }

    [Fact]
    public async Task CalculateSimilarity_WhenInvalidDatasetId_ReturnsBadRequest()
    {
        // Arrange
        int invalidDatasetId = 0; // Invalid ID
        var request = CreateSimilarityRequest(true);

        // Act
        var response = await client.PostAsJsonAsync($"{BaseUrl}/{invalidDatasetId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(content));
    }

    [Fact]
    public async Task CalculateSimilarity_WhenNegativeDatasetId_ReturnsBadRequest()
    {
        // Arrange
        int negativeDatasetId = -5;
        var request = CreateSimilarityRequest(true);

        // Act
        var response = await client.PostAsJsonAsync($"{BaseUrl}/{negativeDatasetId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(content));
    }

    [Fact]
    public async Task CalculateSimilarity_WhenDatasetNotFound_ReturnsNotFound()
    {
        // Arrange
        int nonExistentDatasetId = 99999;
        var request = CreateSimilarityRequest(true);

        // Act
        var response = await client.PostAsJsonAsync($"{BaseUrl}/{nonExistentDatasetId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CalculateSimilarity_WhenDuplicateParameterIds_ReturnsBadRequest()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = CreateSimilarityRequest(
            includeParameters: false,
            parameterSettings: new List<ParameterSettingsDto>
            {
                new() { ParameterId = 1, IsActive = true, Weight = 1.0 },
                new() { ParameterId = 1, IsActive = false, Weight = 0.5 }, // Duplicate ID
                new() { ParameterId = 2, IsActive = true, Weight = 0.8 }
            });

        // Act
        var response = await client.PostAsJsonAsync($"{BaseUrl}/{datasetId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(content));
    }

    [Fact]
    public async Task CalculateSimilarity_WhenInvalidParameterWeights_ReturnsBadRequest()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = CreateSimilarityRequest(
            includeParameters: true,
            parameterSettings: new List<ParameterSettingsDto>
            {
                new() { ParameterId = 1, IsActive = true, Weight = -1.0 }, // Invalid weight
                new() { ParameterId = 2, IsActive = true, Weight = 10.0 }  // Assuming this exceeds max
            });

        // Act
        var response = await client.PostAsJsonAsync($"{BaseUrl}/{datasetId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CalculateSimilarity_WhenNullRequest_ReturnsSuccess()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();

        // Act (null request should be allowed)
        var response = await client.PostAsJsonAsync($"{BaseUrl}/{datasetId}", (SimilarityRequest?)null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SimilarityResult>();
        Assert.NotNull(result);
        Assert.Equal(datasetId, result.DatasetId);
    }

    [Fact]
    public async Task CalculateSimilarity_WhenCachedResultExists_ReturnsOkResult()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = CreateSimilarityRequest(true);

        // First call to create cache
        var firstResponse = await client.PostAsJsonAsync($"{BaseUrl}/{datasetId}", request);
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);

        // Act - Second call should return cached result
        var secondResponse = await client.PostAsJsonAsync($"{BaseUrl}/{datasetId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);
        var result = await secondResponse.Content.ReadFromJsonAsync<SimilarityResult>();
        Assert.NotNull(result);
        Assert.Equal(datasetId, result.DatasetId);
        Assert.NotEmpty(result.Similarities);
    }

    [Fact]
    public async Task CalculateSimilarity_WithComplexParameterSettings_ReturnsSuccess()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = CreateSimilarityRequest(
            includeParameters: true,
            parameterSettings: new List<ParameterSettingsDto>
            {
                new() { ParameterId = 1, IsActive = true, Weight = 2.0 },
                new() { ParameterId = 2, IsActive = false, Weight = 0.5 },
                new() { ParameterId = 3, IsActive = true, Weight = 1.5 },
                new() { ParameterId = 4, IsActive = true, Weight = 0.8 }
            });

        // Act
        var response = await client.PostAsJsonAsync($"{BaseUrl}/{datasetId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SimilarityResult>();
        Assert.NotNull(result);
        Assert.Equal(datasetId, result.DatasetId);
        Assert.NotEmpty(result.Similarities);
    }

    [Fact]
    public async Task CalculateSimilarity_WithMinimalParameterSettings_ReturnsSuccess()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = CreateSimilarityRequest(
            includeParameters: false,
            parameterSettings: new List<ParameterSettingsDto>
            {
                new() { ParameterId = 1, IsActive = true, Weight = 1.0 }
            });

        // Act
        var response = await client.PostAsJsonAsync($"{BaseUrl}/{datasetId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SimilarityResult>();
        Assert.NotNull(result);
        Assert.Equal(datasetId, result.DatasetId);
        Assert.NotEmpty(result.Similarities);
    }

    #region Helper methods

    private static SimilarityRequest CreateSimilarityRequest(
        bool includeParameters,
        List<ParameterSettingsDto>? parameterSettings = null)
    {
        return new SimilarityRequest
        {
            ParameterSettings = parameterSettings ??
            [
                new() { ParameterId = 1, IsActive = true, Weight = 1.0 },
                new() { ParameterId = 2, IsActive = true, Weight = 0.8 }
            ],
            IncludeParameters = includeParameters
        };
    }

    #endregion
}
