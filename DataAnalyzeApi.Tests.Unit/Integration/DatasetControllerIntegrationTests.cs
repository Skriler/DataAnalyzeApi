using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using DataAnalyzeApi.Models.Entities;
using System.Net;
using System.Net.Http.Json;

namespace DataAnalyzeApi.Tests.Integration;

public class DatasetControllerIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task GetAll_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await client.GetAsync("/api/datasets");

        // Assert
        response.EnsureSuccessStatusCode();
        var datasets = await response.Content.ReadFromJsonAsync<List<Dataset>>();
        Assert.NotNull(datasets);
    }

    [Fact]
    public async Task GetById_WhenValidId_ReturnsDataset()
    {
        // Arrange
        var createdDataset = await CreateTestDataset();

        // Act
        var response = await client.GetAsync($"/api/datasets/{createdDataset.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var dataset = await response.Content.ReadFromJsonAsync<DatasetCreateDto>();
        Assert.NotNull(dataset);
        Assert.Equal("Test Dataset", dataset.Name);
    }

    [Fact]
    public async Task GetById_WhenInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await client.GetAsync("/api/datasets/999999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WhenNegativeId_ReturnsBadRequest()
    {
        // Act
        var response = await client.GetAsync("/api/datasets/-1");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenValidData_ReturnsCreated()
    {
        // Arrange
        var dto = CreateTestDatasetDto();

        // Act
        var response = await client.PostAsJsonAsync("/api/datasets", dto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var dataset = await response.Content.ReadFromJsonAsync<Dataset>();
        Assert.NotNull(dataset);
        Assert.Equal("Test Dataset", dataset.Name);
        Assert.True(dataset.Id > 0);
    }

    [Fact]
    public async Task Create_WhenEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var dto = new DatasetCreateDto(
            "",
            new List<string> { "Parameter1", "Parameter2" },
            new List<DataObjectCreateDto>
            {
                new("Object1", new List<string> { "Value1", "Value2" })
            }
        );

        // Act
        var response = await client.PostAsJsonAsync("/api/datasets", dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenEmptyParameters_ReturnsBadRequest()
    {
        // Arrange
        var dto = new DatasetCreateDto(
            "Test Dataset",
            new List<string>(),
            new List<DataObjectCreateDto>
            {
                new("Object1", new List<string> { "Value1", "Value2" })
            }
        );

        // Act
        var response = await client.PostAsJsonAsync("/api/datasets", dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenEmptyObjects_ReturnsBadRequest()
    {
        // Arrange
        var dto = new DatasetCreateDto(
            "Test Dataset",
            new List<string> { "Parameter1", "Parameter2" },
            new List<DataObjectCreateDto>()
        );

        // Act
        var response = await client.PostAsJsonAsync("/api/datasets", dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenNullBody_ReturnsBadRequest()
    {
        // Act
        var response = await client.PostAsJsonAsync("/api/datasets", (DatasetCreateDto?)null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenValidId_ReturnsNoContent()
    {
        // Arrange
        var createdDataset = await CreateTestDataset();

        // Act
        var response = await client.DeleteAsync($"/api/datasets/{createdDataset.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify dataset is deleted
        var getResponse = await client.GetAsync($"/api/datasets/{createdDataset.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await client.DeleteAsync("/api/datasets/999999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenNegativeId_ReturnsBadRequest()
    {
        // Act
        var response = await client.DeleteAsync("/api/datasets/-1");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenRealWorldData_ReturnsCreated()
    {
        // Arrange
        var dto = new DatasetCreateDto(
            "Countries Economic Data",
            new List<string> { "Country", "GDP", "Population", "HDI" },
            new List<DataObjectCreateDto>
            {
                new("USA", new List<string> { "USA", "21.43", "331.9", "0.926" }),
                new("Germany", new List<string> { "Germany", "3.86", "83.2", "0.947" }),
                new("Japan", new List<string> { "Japan", "4.94", "125.8", "0.919" }),
                new("India", new List<string> { "India", "3.39", "1380.0", "0.645" }),
                new("Brazil", new List<string> { "Brazil", "2.26", "213.5", "0.765" })
            }
        );

        // Act
        var response = await client.PostAsJsonAsync("/api/datasets", dto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var dataset = await response.Content.ReadFromJsonAsync<Dataset>();
        Assert.NotNull(dataset);
        Assert.Equal("Countries Economic Data", dataset.Name);
    }

    // Helper methods
    private async Task<Dataset> CreateTestDataset()
    {
        var dto = CreateTestDatasetDto();
        var response = await client.PostAsJsonAsync("/api/datasets", dto);
        response.EnsureSuccessStatusCode();
        var dataset = await response.Content.ReadFromJsonAsync<Dataset>();
        return dataset!;
    }

    private static DatasetCreateDto CreateTestDatasetDto()
    {
        return new DatasetCreateDto(
            "Test Dataset",
            new List<string> { "Parameter1", "Parameter2" },
            new List<DataObjectCreateDto>
            {
                new("Object1", new List<string> { "Value1", "Value2" }),
                new("Object2", new List<string> { "Value3", "Value4" })
            }
        );
    }
}
