namespace DataAnalyzeAPI.Models.DTOs.Analyse.Settings;

public class ParameterSettingsDto
{
    public long ParameterId { get; set; }

    public bool IsActive { get; set; } = true;

    public double Weight { get; set; } = 1.0;
}
