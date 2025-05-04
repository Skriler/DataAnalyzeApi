using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Services.Normalizers.Parameters;

public interface ITypeNormalizer
{
    void AddValue(string value);

    ParameterValueModel Normalize(ParameterValueModel parameterValue);
}
