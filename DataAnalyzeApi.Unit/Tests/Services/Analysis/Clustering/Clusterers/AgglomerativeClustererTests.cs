using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Clustering.Clusterers;
using DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers.Agglomerative;

namespace DataAnalyzeApi.Unit.Tests.Services.Analysis.Clustering.Clusterers;

[Trait("Category", "Unit")]
[Trait("Component", "Analysis")]
[Trait("SubComponent", "Clustering")]
[Trait("Algorithm", "Agglomerative")]
public class AgglomerativeClustererTests : BaseClustererTests<AgglomerativeClusterer, AgglomerativeSettings>
{
    public AgglomerativeClustererTests()
        : base((calculator, generator) => new AgglomerativeClusterer(calculator, generator))
    { }

    private static AgglomerativeSettings CreateAgglomerativeSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric,
        double threshold)
    {
        return new AgglomerativeSettings(
            NumericMetric: numericMetric,
            CategoricalMetric: categoricalMetric,
            IncludeParameters: false,
            Threshold: threshold);
    }

    protected override AgglomerativeSettings CreateDefaultSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric)
    {
        const double threshold = 0.2;

        return CreateAgglomerativeSettings(
            numericMetric,
            categoricalMetric,
            threshold);
    }

    [Theory]
    [MemberData(
        nameof(AgglomerativeClustererTestData.AgglomerativeClustererTestCases),
        MemberType = typeof(AgglomerativeClustererTestData))]
    public void Cluster_ReturnsExpectedDistance(AgglomerativeClustererTestCase testCase)
    {
        var settings = CreateAgglomerativeSettings(
            default,
            default,
            testCase.Threshold);

        ClustererReturnsExpectedClusters(testCase, settings);
    }
}
