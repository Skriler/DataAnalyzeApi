using DataAnalyzeApi.Models.Domain.Dataset.Normalized;
using DataAnalyzeApi.Services.Normalizers.Parameters;
using DataAnalyzeApi.Unit.Common.Factories.Models;

namespace DataAnalyzeApi.Unit.Tests.Services.Normalizers.Parameters;

[Trait("Category", "Unit")]
[Trait("Component", "Normalizer")]
[Trait("SubComponent", "Parameter")]
public class NumericParameterNormalizerTests
{
    private readonly ParameterValueModelFactory valueModelFactory = new();

    [Fact]
    public void Constructor_ShouldAddInitialValue()
    {
        // Arrange & Act
        var normalizer = new NumericParameterNormalizer("10.5");

        // Assert
        Assert.Equal(10.5, normalizer.Min);
        Assert.Equal(10.5, normalizer.Max);
        Assert.Equal(10.5, normalizer.Average);
    }

    [Fact]
    public void AddValue_ShouldThrowException_WhenValueInvalid()
    {
        // Arrange
        var normalizer = new NumericParameterNormalizer("5.0");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => normalizer.AddValue("invalid"));
    }

    [Fact]
    public void Normalize_WhenValidValue_ReturnsNormalizedValue()
    {
        // Arrange
        const double valueA = 0;
        const double valueB = 100;
        const double valueForNormalize = 60;
        // Expected normalized: (60-0) / (100-0) = 0.6
        const double expectedNormalizedValue = 0.6;

        var parameterValue = valueModelFactory.Create(valueForNormalize.ToString());

        var normalizer = new NumericParameterNormalizer(valueA.ToString());
        normalizer.AddValue(valueB.ToString());

        // Act
        var result = normalizer.Normalize(parameterValue) as NormalizedNumericValueModel;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedNormalizedValue, result.NormalizedValue);
        Assert.Equal(parameterValue.Id, result.Id);
        Assert.Equal(parameterValue.Value, result.Value);
        Assert.Equal(parameterValue.ParameterId, result.ParameterId);
    }

    [Fact]
    public void Normalize_WhenEmptyValue_ReturnsAverage()
    {
        // Arrange
        const double valueA = 10;
        const double valueB = 30;
        // Expected normalized: (20-10)/(30-10) = 0.5
        const double expectedNormalizedValue = 0.5;

        var parameterValue = valueModelFactory.Create(string.Empty);

        var normalizer = new NumericParameterNormalizer(valueA.ToString());
        normalizer.AddValue(valueB.ToString());

        // Act
        var result = normalizer.Normalize(parameterValue) as NormalizedNumericValueModel;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedNormalizedValue, result.NormalizedValue);
    }

    [Fact]
    public void Normalize_WhenMinEqualsMax_ReturnsOne()
    {
        // Arrange
        const double value = 42;
        const double expectedNormalizedValue = 1.0;

        var parameterValue = valueModelFactory.Create(value.ToString());
        var normalizer = new NumericParameterNormalizer(value.ToString());

        // Act
        var result = normalizer.Normalize(parameterValue) as NormalizedNumericValueModel;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedNormalizedValue, result.NormalizedValue);
    }
}
