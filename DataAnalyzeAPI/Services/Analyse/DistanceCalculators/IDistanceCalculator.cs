using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Services.Analyse.DistanceCalculators;

public interface IDistanceCalculator
{
    double Calculate(
        List<ParameterValueModel> valuesA,
        List<ParameterValueModel> valuesB,
        NumericDistanceMetricType numericMetricType,
        CategoricalDistanceMetricType categoricalMetricType);
}
