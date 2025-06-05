using System.ComponentModel.DataAnnotations;
using ParamConfig = DataAnalyzeApi.Models.Config.ParameterSettingsConfig;

namespace DataAnalyzeApi.Models.DTOs.Analysis;

public record ParameterSettingsDto
{
    public long ParameterId { get; init; }

    public bool IsActive { get; init; } = ParamConfig.Activity.Default;

    [Range(ParamConfig.Weight.MinAllowed, ParamConfig.Weight.MaxAllowed)]
    public double Weight { get; init; } = ParamConfig.Weight.Default;
}
