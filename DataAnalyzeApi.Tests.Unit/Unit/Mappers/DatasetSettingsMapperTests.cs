using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Tests.Common.Assertions;
using DataAnalyzeApi.Tests.Common.Factories;
using DataAnalyzeApi.Tests.Common.Models;

namespace DataAnalyzeApi.Tests.Unit.Mappers;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
public class DatasetSettingsMapperTests
{
    private readonly DatasetSettingsMapper mapper = new();
    private readonly EntityTestDataFactory entityTestDataFactory = new();
    private readonly DtoTestDataFactory dtoTestDataFactory = new();

    [Theory]
    [InlineData(
        new string[] { "6", "5", "2" },
        new string[] { "8", "9", "2" },
        new string[] { "Width", "Height", "Length" })]
    [InlineData(
        new string[] { "Circle", "Green" },
        new string[] { "Rectangle", "Green" },
        new string[] { "Form", "Height", "Color" })]
    [InlineData(
        new string[] { "Rectangle", "9", "12", "Green" },
        new string[] { "Circle", "5", "3", "Green" },
        new string[] { "Form", "Width", "Height", "Color" })]
    [InlineData(
        new string[] { null!, "3", "" },
        new string[] { "  ", "5", " " },
        new string[] { "Form", "Length", "Color" })]
    public void Map_ReturnsCorrectDatasetModels(string[] valuesA, string[] valuesB, string[] parameterNames)
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

        var dataset = entityTestDataFactory.CreateDataset(rawObjects, parameterNames.ToList());

        // Act
        var datasetModel = mapper.Map(dataset, null!);

        // Assert
        DatasetModelAssertions.AssertDatasetWithSettingsEqualModel(dataset, datasetModel);
    }

    [Theory]
    [InlineData(
        new string[] { "6", "5", "2" },
        new string[] { "8", "9", "2" },
        new string[] { "Width", "Height", "Length" })]
    [InlineData(
        new string[] { "Circle", "Green" },
        new string[] { "Rectangle", "Green" },
        new string[] { "Form", "Height", "Color" })]
    [InlineData(
        new string[] { "17", "3", "Green" },
        new string[] { "6", "4.3", "Red" },
        new string[] { "Width", "Height", "Color" })]
    [InlineData(
        new string[] { null!, "3", "" },
        new string[] { "  ", "5", " " },
        new string[] { "Form", "Length", "Color" })]
    public void Map_WithParameterSettings_ReturnsCorrectDatasetModels(string[] valuesA, string[] valuesB, string[] parameterNames)
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
        var rawParameters = parameterNames.ToList();

        var dataset = entityTestDataFactory.CreateDataset(rawObjects, rawParameters);
        var parameterSettings = dtoTestDataFactory.CreateParameterSettingsDtoList(rawParameters);

        // Act
        var datasetModel = mapper.Map(dataset, parameterSettings);

        // Assert
        DatasetModelAssertions.AssertDatasetWithSettingsEqualModel(dataset, datasetModel, parameterSettings);
    }
}
