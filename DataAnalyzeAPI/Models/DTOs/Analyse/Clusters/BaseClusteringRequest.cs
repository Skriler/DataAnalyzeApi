using DataAnalyzeAPI.Models.DTOs.Analyse.Settings;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clusters;

public abstract class BaseClusteringRequest
{
    public List<ParameterSettingsDto> ParameterSettings { get; set; } = new();
}
