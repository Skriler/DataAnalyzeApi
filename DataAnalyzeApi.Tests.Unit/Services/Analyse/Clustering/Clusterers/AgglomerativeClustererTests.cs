using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Clustering.Clusterers;

public class AgglomerativeClustererTests : BaseClustererTests<AgglomerativeClusterer, AgglomerativeSettings>
{
    protected override AgglomerativeClusterer CreateClusterer(
        IDistanceCalculator calculator,
        ClusterNameGenerator generator)
    {
        return new AgglomerativeClusterer(calculator, generator);
    }

    protected override AgglomerativeSettings CreateSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric)
    {
        const double threshold = 0.2;

        return new AgglomerativeSettings(
            NumericMetric: numericMetric,
            CategoricalMetric: categoricalMetric,
            IncludeParameters: false,
            Threshold: threshold);
    }

    //TODO: add tests
}
