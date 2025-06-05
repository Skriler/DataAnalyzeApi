using DataAnalyzeApi.Exceptions;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Metrics;
using DataAnalyzeApi.Services.Analysis.Metrics.Categorical;
using DataAnalyzeApi.Services.Analysis.Metrics.Numeric;

namespace DataAnalyzeApi.Services.Analysis.Factories.Metric;

public class MetricFactory(IServiceProvider serviceProvider) : IMetricFactory
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    public IDistanceMetric<double> GetNumeric(NumericDistanceMetricType type)
    {
        return type switch
        {
            NumericDistanceMetricType.Euclidean => serviceProvider.GetRequiredService<EuclideanDistanceMetric>(),
            NumericDistanceMetricType.Manhattan => serviceProvider.GetRequiredService<ManhattanDistanceMetric>(),
            NumericDistanceMetricType.Cosine => serviceProvider.GetRequiredService<CosineDistanceMetric>(),
            _ => throw new TypeNotFoundException(nameof(type))
        };
    }

    public IDistanceMetric<int> GetCategorical(CategoricalDistanceMetricType type)
    {
        return type switch
        {
            CategoricalDistanceMetricType.Hamming => serviceProvider.GetRequiredService<HammingDistanceMetric>(),
            CategoricalDistanceMetricType.Jaccard => serviceProvider.GetRequiredService<JaccardDistanceMetric>(),
            _ => throw new TypeNotFoundException(nameof(type))
        };
    }
}
