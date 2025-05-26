using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using DataAnalyzeApi.Models.Entities;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace DataAnalyzeApi.Integration.Tests;

public class DatasetControllerIntegrationTests : IntegrationTestBase
{
    private readonly string BaseUrl = "/api/datasets";

    [Fact]
    public async Task GetAll_ReturnsExpectedDatasetCollection()
    {
        // Act
        var response = await client.GetAsync(BaseUrl);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var datasets = await response.Content.ReadFromJsonAsync<List<Dataset>>();
        Assert.NotNull(datasets);
    }

    [Fact]
    public async Task GetById_WhenValidId_ReturnsExpectedDataset()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();

        // Act
        var response = await client.GetAsync($"{BaseUrl}/{datasetId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var dataset = await response.Content.ReadFromJsonAsync<DatasetCreateDto>();
        Assert.NotNull(dataset);
    }

    [Fact]
    public async Task GetById_WhenNegativeDatasetId_ReturnsBadRequest()
    {
        // Arrange
        const int negativeDatasetId = -5;

        // Act
        var response = await client.GetAsync($"{BaseUrl}/{negativeDatasetId}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WhenNonExistentDataset_ReturnsNotFound()
    {
        // Arrange
        const int nonExistentDatasetId = 99999;

        // Act
        var response = await client.GetAsync($"{BaseUrl}/{nonExistentDatasetId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenValidData_ReturnsExpectedDataset()
    {
        // Arrange
        var jsonDataset = await File.ReadAllTextAsync("Data/technology-companies-analysis-2024.json");
        var datasetDto = JsonConvert.DeserializeObject<DatasetCreateDto>(jsonDataset);

        // Act
        var response = await client.PostAsJsonAsync(BaseUrl, datasetDto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var dataset = await response.Content.ReadFromJsonAsync<Dataset>();
        Assert.NotNull(dataset);
        Assert.True(dataset.Id > 0);
    }

    [Fact]
    public async Task Create_WhenEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var jsonDataset = await File.ReadAllTextAsync("Data/technology-companies-analysis-2024.json");
        var datasetDto = JsonConvert.DeserializeObject<DatasetCreateDto>(jsonDataset)!;
        var emptyNameDatasetDto = datasetDto with { Name = "" };

        // Act
        var response = await client.PostAsJsonAsync(BaseUrl, emptyNameDatasetDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenEmptyParameters_ReturnsBadRequest()
    {
        // Arrange
        var jsonDataset = await File.ReadAllTextAsync("Data/technology-companies-analysis-2024.json");
        var datasetDto = JsonConvert.DeserializeObject<DatasetCreateDto>(jsonDataset)!;
        var emptyParametersDatasetDto = datasetDto with { Parameters = new() };

        // Act
        var response = await client.PostAsJsonAsync(BaseUrl, emptyParametersDatasetDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenEmptyObjects_ReturnsBadRequest()
    {
        // Arrange
        var jsonDataset = await File.ReadAllTextAsync("Data/technology-companies-analysis-2024.json");
        var datasetDto = JsonConvert.DeserializeObject<DatasetCreateDto>(jsonDataset)!;
        var emptyObjectsDatasetDto = datasetDto with { Objects = new() };

        // Act
        var response = await client.PostAsJsonAsync(BaseUrl, emptyObjectsDatasetDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenNullBody_ReturnsBadRequest()
    {
        // Act
        var response = await client.PostAsJsonAsync(BaseUrl, (DatasetCreateDto?)null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenValidId_ReturnsNoContent()
    {
        // Arrange
        var datasetId = await CreateDatasetFromJsonAsync();

        // Act
        var response = await client.DeleteAsync($"{BaseUrl}/{datasetId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify dataset is deleted
        var getResponse = await client.GetAsync($"{BaseUrl}/{datasetId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenNegativeDatasetId_ReturnsBadRequest()
    {
        // Arrange
        const int negativeDatasetId = -5;

        // Act
        var response = await client.DeleteAsync($"{BaseUrl}/{negativeDatasetId}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenNonExistentDataset_ReturnsNotFound()
    {
        // Arrange
        const int nonExistentDatasetId = 99999;

        // Act
        var response = await client.DeleteAsync($"{BaseUrl}/{nonExistentDatasetId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
