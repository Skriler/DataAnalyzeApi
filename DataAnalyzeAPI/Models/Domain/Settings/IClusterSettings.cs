using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Models.Domain.Settings;

public interface IClusterSettings
{
    ClusterAlgorithm GetAlgorithm();
}
