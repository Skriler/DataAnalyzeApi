using DataAnalyzeApi.Services.Analyse.Comparers;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Comparers;

public class NormalizedValueComparerTests
{
    private readonly NormalizedValueComparer comparer = new();

    [Theory]
    [InlineData(null, null, 10.0, 0.0)]
    [InlineData("", "", 10.0, 0.0)]
    [InlineData("   ", "  ", 10.0, 0.0)]
    [InlineData("value", null, 10.0, 0.0)]
    [InlineData(null, "value", 10.0, 0.0)]
    [InlineData("", "value", 10.0, 0.0)]
    public void Compare_WhenNullOrEmptyStrings_ReturnsExpected(string valueA, string valueB, double range, double expected)
    {
        var result = comparer.Compare(valueA, valueB, range);
        Assert.Equal(expected, result, precision: 2);
    }

    [Theory]
    [InlineData("5", "5", 10.0, 1.0)]
    [InlineData("5", "10", 10.0, 0.5)]
    [InlineData("0", "10", 10.0, 0.0)]
    [InlineData("100", "200", 1000.0, 0.9)]
    public void Compare_WhenNumericValues_ReturnsCorrect(string a, string b, double range, double expected)
    {
        var result = comparer.Compare(a, b, range);

        Assert.Equal(expected, result, precision: 3);
    }

    [Theory]
    [InlineData("red", "red", 1.0)]
    [InlineData("red", "RED", 1.0)]
    [InlineData("red", "blue", 0.0)]
    [InlineData("red,blue", "red,blue", 1.0)]
    [InlineData("red,blue", "blue,red", 1.0)]
    [InlineData("red,blue", "red,green", 0.5)]
    [InlineData("red,blue,green", "red,green", 0.67)]
    [InlineData("red, blue", "red,blue", 1.0)]
    public void Compare_WhenCategoricalValues_ReturnsExpected(string a, string b, double expected)
    {
        var result = comparer.Compare(a, b, 10.0);

        Assert.Equal(expected, result, precision: 2);
    }
}
