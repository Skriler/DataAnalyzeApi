using System.ComponentModel.DataAnnotations;
using ParamConfig = DataAnalyzeApi.Models.Config.ParameterSettingsConfig;

namespace DataAnalyzeApi.Models.DTOs.Analyse.Settings;

public record ParameterSettingsDto
{
    public long ParameterId { get; set; }

    public bool IsActive { get; set; } = ParamConfig.Activity.Default;

    [Range(ParamConfig.Weight.MinAllowed, ParamConfig.Weight.MaxAllowed)]
    public double Weight { get; set; } = ParamConfig.Weight.Default;
}
