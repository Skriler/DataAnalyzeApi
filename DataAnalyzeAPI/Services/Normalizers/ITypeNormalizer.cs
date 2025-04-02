using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeAPI.Services.Normalizers;

public interface ITypeNormalizer
{
    long ParameterId { get; }

    void AddValue(string value);

    ParameterValueModel Normalize(ParameterValueModel parameterValue);
}
