using DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;

namespace DataAnalyzeAPI.Services.Normalizers;

public interface ITypeNormalizer
{
    long ParameterId { get; }

    void AddValue(string value);

    ParameterValueDto Normalize(ParameterValueDto parameterValue);
}
