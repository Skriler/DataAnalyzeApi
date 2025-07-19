using AutoMapper;
using DataAnalyzeApi.Mappers.Entities.Profiles;
using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Unit.Common.Assertions.Datasets;
using DataAnalyzeApi.Unit.Common.Factories.Datasets;
using DataAnalyzeApi.Unit.Common.Models.Analysis;

namespace DataAnalyzeApi.Unit.Tests.Mappers.Entities.Profile;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
[Trait("SubComponent", "Entities")]
public class DatasetCreateProfileTests
{
    private readonly IMapper mapper;
    private readonly DatasetDtoTestFactory dtoTestDataFactory;
    private readonly DatasetEntityTestFactory entityTestDataFactory;

    public DatasetCreateProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<DatasetCreateProfile>());

        mapper = configuration.CreateMapper();
        dtoTestDataFactory = new DatasetDtoTestFactory();
        entityTestDataFactory = new DatasetEntityTestFactory();
    }

    [Theory]
    [InlineData(
        new string[] { "6", "5", "2" },
        new string[] { "8", "9", "2" },
        new string[] { "Width", "Height", "Length" })]
    [InlineData(
        new string[] { "Circle", "Green" },
        new string[] { "Rectangle", "Green" },
        new string[] { "Form", "Color" })]
    [InlineData(
        new string[] { "Rectangle", "9", "12", "Green" },
        new string[] { "Circle", "5", "3", "Green" },
        new string[] { "Form", "Width", "Height", "Color" })]
    [InlineData(
        new string[] { null!, "3", "" },
        new string[] { "  ", "5", " " },
        new string[] { "Form", "Length", "Color" })]
    public void MapToDataset_ReturnsCorrectDataset(
        string[] valuesA,
        string[] valuesB,
        string[] parameterNames)
    {
        // Arrange
        var rawObjects = new List<RawDataObject>()
        {
            new()
            {
                Values = valuesA.ToList(),
            },

            new()
            {
                Values = valuesB.ToList(),
            },
        };

        var createDto = dtoTestDataFactory.CreateDatasetCreateDto(
            rawObjects,
            parameterNames.ToList());

        // Act
        var dataset = mapper.Map<Dataset>(createDto);

        // Assert
        DatasetDtoAssertions.AssertDatasetEqualCreateDto(createDto, dataset);
    }

    [Theory]
    [InlineData(
        new string[] { "6", "5", "2" },
        new string[] { "8", "9", "2" },
        new string[] { "Width", "Height", "Length" })]
    [InlineData(
        new string[] { "Circle", "Green" },
        new string[] { "Rectangle", "Green" },
        new string[] { "Form", "Color" })]
    [InlineData(
        new string[] { "17", "3", "Green" },
        new string[] { "6", "4.3", "Red" },
        new string[] { "Width", "Height", "Color" })]
    [InlineData(
        new string[] { null!, "3", "" },
        new string[] { "  ", "5", " " },
        new string[] { "Form", "Length", "Color" })]
    public void MapToDatasetCreateDto_ReturnsCorrectDatasetDto(
        string[] valuesA,
        string[] valuesB,
        string[] parameterNames)
    {
        // Arrange
        var rawObjects = new List<RawDataObject>()
        {
            new()
            {
                Values = valuesA.ToList(),
            },

            new()
            {
                Values = valuesB.ToList(),
            },
        };

        var dataset = entityTestDataFactory.CreateDataset(
            rawObjects,
            parameterNames.ToList());

        // Act
        var createDto = mapper.Map<DatasetCreateDto>(dataset);

        // Assert
        DatasetDtoAssertions.AssertDatasetEqualCreateDto(createDto, dataset);
    }
}
