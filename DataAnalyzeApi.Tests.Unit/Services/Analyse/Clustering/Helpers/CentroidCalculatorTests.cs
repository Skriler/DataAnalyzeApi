using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Helpers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Clustering.Helpers;

public class CentroidCalculatorTests
{
    private readonly TestDataFactory dataFactory = new();
    private readonly CentroidCalculator calculator = new();

    [Fact]
    public void Recalculate_ShouldThrowException_WhenObjectsHaveDifferentParameterCount()
    {
        // Arrange
        var centroidNumerics = new NormalizedDataObject() { NumericValues = { 0.2, 0.6, 0.4 } };
        var objectNumerics = new NormalizedDataObject() { NumericValues = { 0.3, 0.4 } };

        var centroid = dataFactory.CreateCentroid(centroidNumerics);
        var dataObjects = new List<DataObjectModel>
        {
            dataFactory.CreateNormalizedDataObjectModel(objectNumerics)
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => calculator.Recalculate(centroid, dataObjects));
    }

    [Theory]
    [MemberData(nameof(RecalculateTestCases))]
    public void Calculate_ReturnsExpectedDistance(CentroidTestCase testCase)
    {
        // Arrange
        var centroid = dataFactory.CreateCentroid(testCase.InitialCentroid);
        var dataset = dataFactory.CreateNormalizedDatasetModel(testCase.Objects);
        var expectedCentroid = dataFactory.CreateCentroid(testCase.ExpectedCentroid);

        // Act
        var result = calculator.Recalculate(centroid, dataset.Objects);

        // Assert
        Assert.NotEmpty(result.Values);
        ParameterValueComparison.AssertParameterValuesEqual(expectedCentroid.Values, result.Values);
    }

    public static IEnumerable<object[]> RecalculateTestCases => CentroidCalculatorTestData.GetRecalculateTestCases();
}
