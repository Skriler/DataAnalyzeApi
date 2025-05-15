using DataAnalyzeApi.Services.Normalizers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Helpers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Normalizers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;

namespace DataAnalyzeApi.Tests.Unit.Services.Normalizers;

public class DatasetNormalizerTests
{
    private readonly ServiceDataFactory dataFactory = new();

    [Theory]
    [MemberData(nameof(DatasetNormalizerTestData.GetNormalizeTestCases), MemberType = typeof(DatasetNormalizerTestData))]
    public void Normalize_WhenMixedParameters_ReturnsCorrectNormalization(NormalizerTestCase testCase)
    {
        // Arrange
        var dataset = dataFactory.CreateDatasetModel(testCase.RawObjects);
        var expecteDataset = dataFactory.CreateNormalizedDatasetModel(testCase.NormalizedObjects);
        var normalizer = new DatasetNormalizer();

        // Act
        var result = normalizer.Normalize(dataset);

        // Assert
        Assert.NotEmpty(result.Objects);
        ParameterValueComparison.AssertDataObjectsEqual(expecteDataset.Objects, result.Objects);
    }
}
