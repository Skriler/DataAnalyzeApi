using DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Requests;
using DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings;
using DataAnalyzeApi.Models.Enums;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace DataAnalyzeApi.Tests.Integration;

public class ClusteringControllerIntegrationTests : IntegrationTestBase
{
    private readonly string BaseUrl = "/api/analyse/clustering";

    [Fact]
    public async Task CalculateKMeansClusters_WhenValidRequest_ReturnsOk()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = CreateKMeansRequest(
            includeParameters: true,
            numberOfClusters: 3,
            maxIterations: 100);

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/kmeans/{datasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ClusteringResult>(responseContent);

        Assert.NotNull(result);
        Assert.NotNull(result.Clusters);
        Assert.True(result.Clusters.Count > 0);
    }

    [Fact]
    public async Task CalculateDBSCANClusters_WhenValidRequest_ReturnsOk()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = CreateDBSCANRequest(
            includeParameters: true,
            epsilon: 0.5,
            minPoints: 5);

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/dbscan/{datasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ClusteringResult>(responseContent);

        Assert.NotNull(result);
        Assert.NotNull(result.Clusters);
    }

    [Fact]
    public async Task CalculateAgglomerativeClusters_WhenValidRequest_ReturnsOk()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = CreateAgglomerativeRequest(
            includeParameters: true,
            threshold: 0.8);

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/agglomerative/{datasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ClusteringResult>(responseContent);

        Assert.NotNull(result);
        Assert.NotNull(result.Clusters);
    }

    [Fact]
    public async Task CalculateKMeansClusters_WhenInvalidDatasetId_ReturnsBadRequest()
    {
        // Arrange
        int invalidDatasetId = -1;
        var request = CreateKMeansRequest(
            includeParameters: false,
            numberOfClusters: 3,
            maxIterations: 100);

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/kmeans/{invalidDatasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CalculateDBSCANClusters_WhenInvalidDatasetId_ReturnsBadRequest()
    {
        // Arrange
        int invalidDatasetId = 0;
        var request = CreateDBSCANRequest(
            includeParameters: false,
            epsilon: 0.5,
            minPoints: 5);

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/dbscan/{invalidDatasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CalculateAgglomerativeClusters_WhenInvalidDatasetId_ReturnsBadRequest()
    {
        // Arrange
        int invalidDatasetId = -5;
        var request = CreateAgglomerativeRequest(
            includeParameters: false,
            threshold: 0.8);

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/agglomerative/{invalidDatasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CalculateKMeansClusters_WhenNonExistentDataset_ReturnsNotFound()
    {
        // Arrange
        int nonExistentDatasetId = 99999;
        var request = CreateKMeansRequest(
            includeParameters: false,
            numberOfClusters: 3,
            maxIterations: 100);

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/kmeans/{nonExistentDatasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CalculateDBSCANClusters_WhenNonExistentDataset_ReturnsNotFound()
    {
        // Arrange
        int nonExistentDatasetId = 99999;
        var request = CreateDBSCANRequest(
            includeParameters: false,
            epsilon: 0.5,
            minPoints: 5);

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/dbscan/{nonExistentDatasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CalculateAgglomerativeClusters_WhenNonExistentDataset_ReturnsNotFound()
    {
        // Arrange
        int nonExistentDatasetId = 99999;
        var request = CreateAgglomerativeRequest(
            includeParameters: false,
            threshold: 0.8);

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/agglomerative/{nonExistentDatasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CalculateKMeansClusters_WhenInvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var invalidRequest = new KMeansClusteringRequest
        {
            NumericMetric = NumericDistanceMetricType.Euclidean,
            CategoricalMetric = CategoricalDistanceMetricType.Hamming,
            NumberOfClusters = -1, // Invalid cluster count (below minimum)
            MaxIterations = 2000, // Invalid iteration count (above maximum)
            ParameterSettings = new List<ParameterSettingsDto>
            {
                new() { ParameterId = 1, Weight = -1.0 } // Invalid weight (below minimum)
            }
        };

        var json = JsonConvert.SerializeObject(invalidRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/kmeans/{datasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CalculateDBSCANClusters_WhenInvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var invalidRequest = new DBSCANClusteringRequest
        {
            NumericMetric = NumericDistanceMetricType.Euclidean,
            CategoricalMetric = CategoricalDistanceMetricType.Hamming,
            Epsilon = -0.5, // Invalid epsilon (below minimum)
            MinPoints = 200, // Invalid min points (above maximum)
            ParameterSettings = new List<ParameterSettingsDto>
            {
                new() { ParameterId = 1, Weight = 15.0 } // Invalid weight (above maximum)
            }
        };

        var json = JsonConvert.SerializeObject(invalidRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/dbscan/{datasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CalculateAgglomerativeClusters_WhenInvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var invalidRequest = new AgglomerativeClusteringRequest
        {
            NumericMetric = NumericDistanceMetricType.Euclidean,
            CategoricalMetric = CategoricalDistanceMetricType.Hamming,
            Threshold = 1.5, // Invalid threshold (above maximum)
            ParameterSettings = new List<ParameterSettingsDto>
            {
                new() { ParameterId = 1, Weight = -5.0 } // Invalid weight (below minimum)
            }
        };

        var json = JsonConvert.SerializeObject(invalidRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/agglomerative/{datasetId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CalculateKMeansClusters_WhenCachedResult_ReturnsOkFromCache()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();
        var request = CreateKMeansRequest(
            includeParameters: true,
            numberOfClusters: 3,
            maxIterations: 100);

        var json = JsonConvert.SerializeObject(request);
        var firstContent = new StringContent(json, Encoding.UTF8, "application/json");
        var secondContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Act - First request (should calculate and cache)
        var firstResponse = await client.PostAsync($"{BaseUrl}/kmeans/{datasetId}", firstContent);

        // Act - Second request (should return cached result)
        var secondResponse = await client.PostAsync($"{BaseUrl}/kmeans/{datasetId}", secondContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);

        var result1 = JsonConvert.DeserializeObject<ClusteringResult>(await firstResponse.Content.ReadAsStringAsync());
        var result2 = JsonConvert.DeserializeObject<ClusteringResult>(await secondResponse.Content.ReadAsStringAsync());

        Assert.NotNull(result1);
        Assert.NotNull(result2);
        // Results should be identical for cached response
        Assert.Equal(result1.Clusters?.Count, result2.Clusters?.Count);
    }

    #region Helper methods

    private static KMeansClusteringRequest CreateKMeansRequest(
        bool includeParameters,
        int numberOfClusters,
        int maxIterations,
        List<ParameterSettingsDto>? parameterSettings = null)
    {
        return new KMeansClusteringRequest
        {
            NumericMetric = NumericDistanceMetricType.Euclidean,
            CategoricalMetric = CategoricalDistanceMetricType.Hamming,
            IncludeParameters = includeParameters,
            MaxIterations = maxIterations,
            NumberOfClusters = numberOfClusters,
            ParameterSettings = parameterSettings ??
            [
                new() { ParameterId = 1, IsActive = true, Weight = 1.0 },
                new() { ParameterId = 2, IsActive = true, Weight = 1.5 }
            ]
        };
    }

    private static DBSCANClusteringRequest CreateDBSCANRequest(
        bool includeParameters,
        double epsilon,
        int minPoints,
        List<ParameterSettingsDto>? parameterSettings = null)
    {
        return new DBSCANClusteringRequest
        {
            NumericMetric = NumericDistanceMetricType.Euclidean,
            CategoricalMetric = CategoricalDistanceMetricType.Hamming,
            IncludeParameters = includeParameters,
            Epsilon = epsilon,
            MinPoints = minPoints,
            ParameterSettings = parameterSettings ?? new List<ParameterSettingsDto>
            {
                new() { ParameterId = 1, IsActive = true, Weight = 1.0 },
                new() { ParameterId = 2, IsActive = true, Weight = 1.5 }
            }
        };
    }

    private static AgglomerativeClusteringRequest CreateAgglomerativeRequest(
        bool includeParameters,
        double threshold,
        List<ParameterSettingsDto>? parameterSettings = null)
    {
        return new AgglomerativeClusteringRequest
        {
            NumericMetric = NumericDistanceMetricType.Euclidean,
            CategoricalMetric = CategoricalDistanceMetricType.Hamming,
            IncludeParameters = includeParameters,
            Threshold = threshold,
            ParameterSettings = parameterSettings ??
            [
                new() { ParameterId = 1, IsActive = true, Weight = 1.0 },
                new() { ParameterId = 2, IsActive = true, Weight = 1.5 }
            ]
        };
    }

    #endregion
}
