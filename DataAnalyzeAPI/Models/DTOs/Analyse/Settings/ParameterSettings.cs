using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Settings;

public class ParameterSettings
{
    public long ParameterId { get; set; }

    public bool IsActive { get; set; } = true;

    [Range(0, 10)]
    public double Weight { get; set; } = 1.0;
}
