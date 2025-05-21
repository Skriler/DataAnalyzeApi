using DataAnalyzeApi.Models.Entities;
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
}
