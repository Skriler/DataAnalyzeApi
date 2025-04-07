using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Services.Analyse.DistanceCalculators;

public interface IDistanceCalculator
{
    double Calculate(
        DataObjectModel objectA,
        DataObjectModel objectB,
        NumericDistanceMetricType numericMetricType,
        CategoricalDistanceMetricType categoricalMetricType);
}
