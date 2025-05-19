using DataAnalyzeApi.Services.Normalizers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Normalizers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Assertions;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories.Models;

namespace DataAnalyzeApi.Tests.Unit.Services.Normalizers;

public class DatasetNormalizerTests
{
    private readonly DatasetModelFactory datasetModelFactory = new();

    [Theory]
    [MemberData(nameof(DatasetNormalizerTestData.GetNormalizeTestCases), MemberType = typeof(DatasetNormalizerTestData))]
    public void Normalize_WhenMixedParameters_ReturnsCorrectNormalization(NormalizerTestCase testCase)
    {
        // Arrange
        var dataset = datasetModelFactory.Create(testCase.RawObjects);

        // TODO: fix parameters creation
        var expecteDataset = datasetModelFactory.CreateNormalized(testCase.NormalizedObjects);
        var normalizer = new DatasetNormalizer();

        // Act
        var result = normalizer.Normalize(dataset);

        // Assert
        Assert.NotEmpty(result.Objects);
        DatasetAssertions.AssertDataObjectsEqual(expecteDataset.Objects, result.Objects);
    }
}
