using DataAnalyzeApi.Services.Validation;
using DataAnalyzeApi.Unit.Common.Factories;
using DataAnalyzeApi.Unit.Common.Models.Analysis;

namespace DataAnalyzeApi.Unit.Tests.Services.Validation;

[Trait("Category", "Unit")]
[Trait("Component", "Validation")]
[Trait("SubComponent", "DatasetValidator")]
public class DatasetValidatorTests
{
    private readonly DtoTestDataFactory factory = new();
    private readonly DatasetValidator validator = new();

    [Fact]
    public void ValidateDataset_ShouldReturnInvalid_WhenDatasetIsNull()
    {
        // Act
        var result = validator.ValidateDataset(null!);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void ValidateDataset_ShouldReturnInvalid_WhenParametersAreEmpty()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = ["1.5"] },
        };
        var parameters = new List<string>();

        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void ValidateDataset_ShouldReturnInvalid_WhenObjectsAreEmpty()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>();
        var parameters = new List<string> { "Parameter1" };
        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void ValidateDataset_ShouldReturnInvalid_WhenParametersContainDuplicates()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = ["1.5", "2.0"] },
        };
        var parameters = new List<string> { "Parameter1", "parameter1" };
        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void ValidateDataset_ShouldReturnInvalid_WhenObjectNamesContainDuplicates()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = ["1.5"] },
            new() { Values = ["2.0"] },
        };
        var parameters = new List<string> { "Parameter1" };
        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        dto.Objects[0] = dto.Objects[0] with { Name = "Object1" };
        dto.Objects[1] = dto.Objects[1] with { Name = "object1" };

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void ValidateDataset_ShouldReturnInvalid_WhenObjectHasIncorrectValueCount()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = ["1.5", "2.0"] },
            new() { Values = ["3.0"] },
        };
        var parameters = new List<string> { "Parameter1", "Parameter2" };
        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void ValidateDataset_ShouldReturnInvalid_WhenMixedDataTypes()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = ["1.5"] },
            new() { Values = ["Category"] },
        };
        var parameters = new List<string> { "Parameter1" };

        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void ValidateDataset_WhenDatasetIsValid_ReturnsValid()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = ["1.5", "Category1"] },
            new() { Values = ["2.0", "Category2"] },
        };
        var parameters = new List<string> { "Parameter1", "Parameter2" };

        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateDataset_WhenOnlyNumericValues_ReturnsValid()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = ["1.5"] },
            new() { Values = ["2.0"] },
            new() { Values = ["3.5"] },
        };
        var parameters = new List<string> { "Parameter1" };

        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateDataset_WhenCategoricalValues_ReturnsValid()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = ["Category1"] },
            new() { Values = ["Category2"] },
            new() { Values = ["Category3"] }
        };
        var parameters = new List<string> { "Parameter1" };
        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateDataset_WhenParameterHasEmptyValues_ReturnsValid()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = [""] },
            new() { Values = [" "] },
            new() { Values = [null!] },
        };
        var parameters = new List<string> { "Parameter1" };

        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateDataset_WhenHasEmptyValues_ReturnsValid()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = ["1.5"] },
            new() { Values = [""] },
            new() { Values = ["2.0"] },
        };
        var parameters = new List<string> { "Parameter1" };
        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateDataset_WhenParametersHaveConsistentTypes_ReturnsValid()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new() { Values = ["1.5", "CategoryA", "10"] },
            new() { Values = ["2.0", "CategoryB", "20"] },
            new() { Values = ["3.5", "CategoryC", "30"] },
        };
        var parameters = new List<string> { "Numeric1", "Categorical", "Numeric2" };

        var dto = factory.CreateDatasetCreateDto(rawObjects, parameters);

        // Act
        var result = validator.ValidateDataset(dto);

        // Assert
        Assert.True(result.IsValid);
    }
}
