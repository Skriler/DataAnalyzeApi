using AutoMapper;
using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;

namespace DataAnalyzeApi.Tests.Unit.Mappers;

public class DatasetSettingsMapperTests
{
    private readonly DatasetSettingsMapper mapper = new();
    private readonly MapperDataFactory dataFactory = new();

    [Theory]
    [InlineData(new string[] { "6", "5", "2" }, new string[] { "8", "9", "2" }, new string[] { "Width", "Height", "Length" })]
    [InlineData(new string[] { "Circle", "Green" }, new string[] { "Rectangle", "Green" }, new string[] { "Form", "Height", "Color" })]
    [InlineData(new string[] { null!, "3", "" }, new string[] { "  ", "5", " " }, new string[] { "Form", "Length", "Color" })]
    public void Map_ReturnsDatasetModels(string[] valuesA, string[] valuesB, string[] parameterNames)
    {
        // Arrange
        var rawObjects = new List<RawDataObject>()
        {
            new RawDataObject
            {
                Values = valuesA.ToList(),
            },

            new RawDataObject
            {
                Values = valuesB.ToList(),
            },
        };

        var dataset = dataFactory.CreateDatasetEntity(rawObjects, parameterNames.ToList());

        // Act
        var datasetModel = mapper.Map(dataset, null!);

        // Assert
    }
}
