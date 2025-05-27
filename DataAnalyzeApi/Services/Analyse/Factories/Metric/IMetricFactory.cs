using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Metrics;

namespace DataAnalyzeApi.Services.Analyse.Factories.Metric;

public interface IMetricFactory
{
    IDistanceMetric<double> GetNumeric(NumericDistanceMetricType type);

    IDistanceMetric<int> GetCategorical(CategoricalDistanceMetricType type);
}
