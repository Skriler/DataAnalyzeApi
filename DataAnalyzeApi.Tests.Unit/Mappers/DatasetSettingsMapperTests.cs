using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories;
namespace DataAnalyzeApi.Tests.Unit.Mappers;

public class DatasetSettingsMapperTests
{
    private readonly DatasetSettingsMapper mapper = new();
    private readonly EntityTestDataFactory entityTestDataFactory = new();
    private readonly DtoTestDataFactory dtoTestDataFactory = new();

    [Theory]
    [InlineData(new string[] { "6", "5", "2" }, new string[] { "8", "9", "2" }, new string[] { "Width", "Height", "Length" })]
    [InlineData(new string[] { "Circle", "Green" }, new string[] { "Rectangle", "Green" }, new string[] { "Form", "Height", "Color" })]
    [InlineData(new string[] { null!, "3", "" }, new string[] { "  ", "5", " " }, new string[] { "Form", "Length", "Color" })]
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
    }

    [Theory]
    [InlineData(new string[] { "6", "5", "2" }, new string[] { "8", "9", "2" }, new string[] { "Width", "Height", "Length" })]
    [InlineData(new string[] { "Circle", "Green" }, new string[] { "Rectangle", "Green" }, new string[] { "Form", "Height", "Color" })]
    [InlineData(new string[] { null!, "3", "" }, new string[] { "  ", "5", " " }, new string[] { "Form", "Length", "Color" })]
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
    }
}
