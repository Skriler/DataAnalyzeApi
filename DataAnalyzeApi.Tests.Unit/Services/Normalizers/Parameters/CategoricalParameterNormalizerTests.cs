using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;
using DataAnalyzeApi.Services.Normalizers.Parameters;
namespace DataAnalyzeApi.Tests.Unit.Services.Normalizers.Parameters;

public class CategoricalParameterNormalizerTests
{
    [Fact]
    public void Constructor_WhenEmptyValue_ShouldCreateEmptyCategories()
    {
        // Arrange & Act
        var normalizer = new CategoricalParameterNormalizer(string.Empty);

        // Assert
        Assert.Empty(normalizer.Categories);
    }

    [Fact]
    public void AddValue_WhenDuplicates_ShouldIgnoreDuplicates()
    {
        // Arrange
        var normalizer = new CategoricalParameterNormalizer("Red, Green");

        // Act
        normalizer.AddValue("Green, Blue");
        normalizer.AddValue("Red, Purple");

        // Assert
        Assert.Equal(4, normalizer.Categories.Count);
        Assert.Contains("Red", normalizer.Categories);
        Assert.Contains("Green", normalizer.Categories);
        Assert.Contains("Blue", normalizer.Categories);
        Assert.Contains("Purple", normalizer.Categories);
    }

    [Fact]
    public void Normalize_WhenValidValue_ReturnsCorrectOneHot()
    {
        // Arrange
        const string categories = "Red, Green, Blue, Yellow";
        const string categoriesForNormalize = "Red, Blue";
        int[] expectedNormalizedValue = { 1, 0, 1, 0 };

        var normalizer = new CategoricalParameterNormalizer(categories);
        var parameterValue = CreateParameterValue(categoriesForNormalize);

        // Act
        var result = normalizer.Normalize(parameterValue) as NormalizedCategoricalValueModel;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedNormalizedValue.Length, result.OneHotValues.Length);
        Assert.Equal(expectedNormalizedValue, result.OneHotValues);
    }

    [Fact]
    public void Normalize_WhenEmptyValueProvided_ReturnsZeroArray()
    {
        // Arrange
        const string categories = "Red, Green, Blue";
        int[] expectedNormalizedValue = { 0, 0, 0 };

        var normalizer = new CategoricalParameterNormalizer(categories);
        var parameterValue = CreateParameterValue(string.Empty);

        // Act
        var result = normalizer.Normalize(parameterValue) as NormalizedCategoricalValueModel;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedNormalizedValue.Length, result.OneHotValues.Length);
        Assert.Equal(expectedNormalizedValue, result.OneHotValues);
    }

    [Fact]
    public void Normalize_WhenUnknownValueProvided_ShouldIgnoreUnknown()
    {
        // Arrange
        const string categories = "Red, Green, Blue";
        const string categoriesForNormalize = "Purple, Red";
        int[] expectedNormalizedValue = { 1, 0, 0 };

        var normalizer = new CategoricalParameterNormalizer(categories);
        var parameterValue = CreateParameterValue(categoriesForNormalize);

        // Act
        var result = normalizer.Normalize(parameterValue) as NormalizedCategoricalValueModel;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedNormalizedValue.Length, result.OneHotValues.Length);
        Assert.Equal(expectedNormalizedValue, result.OneHotValues);
    }

    #region Test Helpers

    private static ParameterValueModel CreateParameterValue(string value, long id = 1)
    {
        return new ParameterValueModel(
            Id: id,
            Value: value,
            ParameterId: id,
            Parameter: null!);
    }

    #endregion
}
