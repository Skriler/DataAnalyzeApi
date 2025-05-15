using AutoMapper;
using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;

namespace DataAnalyzeApi.Tests.Unit.Mappers;

public class DatasetProfileTests
{
    private readonly IMapper mapper;
    private readonly MapperDataFactory dataFactory;

    public DatasetProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<DatasetProfile>());

        mapper = configuration.CreateMapper();
        dataFactory = new MapperDataFactory();
    }

    [Theory]
    [InlineData(new string[] { "6", "5", "2" }, new string[] { "8", "9", "2" })]
    [InlineData(new string[] { "Circle", "Green" }, new string[] { "Rectangle", "Green" })]
    [InlineData(new string[] { null!, "3", "" }, new string[] { "  ", "5", " " })]
    public void MapToDataset_ReturnsCorrectDataset(string[] valuesA, string[] valuesB)
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

        var dto = dataFactory.CreateDatasetCreateDto(rawObjects);

        // Act
        var dataset = mapper.Map<Dataset>(dto);

        // Assert
        Assert.Equal(dto.Name, dataset.Name);
        Assert.Equal(dto.Parameters.Count, dataset.Parameters.Count);
        Assert.Equal(rawObjects.Count, dataset.Objects.Count);
        Assert.Equal(dto.Objects[0].Values.Count, dataset.Objects[0].Values.Count);
    }

    [Theory]
    [InlineData(new string[] { "6", "5", "2" }, new string[] { "8", "9", "2" }, new string[] { "Width", "Height", "Length" })]
    [InlineData(new string[] { "Circle", "Green" }, new string[] { "Rectangle", "Green" }, new string[] { "Form", "Height", "Color" })]
    [InlineData(new string[] { null!, "3", "" }, new string[] { "  ", "5", " " }, new string[] { "Form", "Length", "Color" })]
    public void MapToDatasetCreateDto_ReturnsCorrectDatasetDto(
        string[] valuesA,
        string[] valuesB,
        string[] parameterNames)
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
        var dto = mapper.Map<Dataset>(dataset);

        // Assert
        Assert.Equal(dataset.Name, dto.Name);
        Assert.Equal(dataset.Parameters.Count, dto.Parameters.Count);
        Assert.Equal(dataset.Objects.Count, dto.Objects.Count);
        Assert.Equal(dataset.Objects[0].Values.Count, dto.Objects[0].Values.Count);
    }
}
