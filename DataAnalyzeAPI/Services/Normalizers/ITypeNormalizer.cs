using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Services.Normalizers;

public interface ITypeNormalizer
{
    long ParameterId { get; }

    void AddValue(string value);

    ParameterValueModel Normalize(ParameterValueModel parameterValue);
}
