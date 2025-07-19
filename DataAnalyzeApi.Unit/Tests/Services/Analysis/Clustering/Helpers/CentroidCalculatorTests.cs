using DataAnalyzeApi.Services.Analysis.Clustering.Helpers;
using DataAnalyzeApi.Unit.Common.Assertions.Datasets;
using DataAnalyzeApi.Unit.Common.Factories;
using DataAnalyzeApi.Unit.Common.Factories.Datasets.Models;
using DataAnalyzeApi.Unit.Common.Models.Analysis;
using DataAnalyzeApi.Unit.Common.TestData.Clustering.Helpers;

namespace DataAnalyzeApi.Unit.Tests.Services.Analysis.Clustering.Helpers;

[Trait("Category", "Unit")]
[Trait("Component", "Analysis")]
[Trait("SubComponent", "Clustering")]
public class CentroidCalculatorTests
{
    private readonly CentroidCalculator calculator = new();
    private readonly CentroidFactory centroidFactory = new();
    private readonly DatasetModelFactory datasetModelFactory = new();

    [Fact]
    public void Recalculate_ShouldThrowException_WhenObjectsHaveDifferentParameterCount()
    {
        // Arrange
        var centroidNumerics = new NormalizedDataObject
        {
            NumericValues = [0.2, 0.6, 0.4]
        };

        var objects = new List<NormalizedDataObject>
        {
            new()
            {
                NumericValues = [0.3, 0.4]
            },
        };

        var centroid = centroidFactory.Create(centroidNumerics);
        var dataset = datasetModelFactory.CreateNormalized(objects);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => calculator.Recalculate(centroid, dataset.Objects));
    }

    [Theory]
    [MemberData(
        nameof(CentroidCalculatorTestData.RecalculateTestCases),
        MemberType = typeof(CentroidCalculatorTestData))]
    public void Calculate_ReturnsExpectedDistance(CentroidCalculatorTestCase testCase)
    {
        // Arrange
        var centroid = centroidFactory.Create(testCase.InitialCentroid);
        var dataset = datasetModelFactory.CreateNormalized(testCase.Objects);
        var expectedCentroid = centroidFactory.Create(testCase.ExpectedCentroid);

        // Act
        var result = calculator.Recalculate(centroid, dataset.Objects);

        // Assert
        Assert.NotEmpty(result.Values);
        DatasetAssertions.AssertParameterValuesEqual(expectedCentroid.Values, result.Values);
    }
}
