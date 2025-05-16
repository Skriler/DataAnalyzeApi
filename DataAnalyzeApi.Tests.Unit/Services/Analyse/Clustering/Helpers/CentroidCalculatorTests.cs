using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Clusterers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Helpers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Assertions;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories.Models;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Clustering.Helpers;

public class CentroidCalculatorTests
{
    private readonly CentroidCalculator calculator = new();
    private readonly CentroidFactory centroidFactory = new();
    private readonly DatasetModelFactory datasetModelFactory = new();

    [Fact]
    public void Recalculate_ShouldThrowException_WhenObjectsHaveDifferentParameterCount()
    {
        // Arrange
        var centroidNumerics = new NormalizedDataObject() { NumericValues = { 0.2, 0.6, 0.4 } };
        var objectNumerics = new NormalizedDataObject() { NumericValues = { 0.3, 0.4 } };

        var centroid = centroidFactory.Create(centroidNumerics);
        var dataObjects = new List<DataObjectModel>
        {
            datasetModelFactory.CreateNormalizedDataObjectModel(objectNumerics)
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => calculator.Recalculate(centroid, dataObjects));
    }

    [Theory]
    [MemberData(nameof(CentroidCalculatorTestData.GetRecalculateTestCases), MemberType = typeof(CentroidCalculatorTestData))]
    public void Calculate_ReturnsExpectedDistance(CentroidTestCase testCase)
    {
        // Arrange
        var centroid = centroidFactory.Create(testCase.InitialCentroid);
        var dataset = dataFactory.CreateNormalizedDatasetModel(testCase.Objects);
        var expectedCentroid = centroidFactory.Create(testCase.ExpectedCentroid);

        // Act
        var result = calculator.Recalculate(centroid, dataset.Objects);

        // Assert
        Assert.NotEmpty(result.Values);
        DatasetAssertions.AssertParameterValuesEqual(expectedCentroid.Values, result.Values);
    }
}
