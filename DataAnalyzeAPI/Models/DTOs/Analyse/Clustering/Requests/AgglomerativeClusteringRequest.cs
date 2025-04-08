using DataAnalyzeAPI.Models.Config.Clustering;
using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;

public class AgglomerativeClusteringRequest : BaseClusteringRequest
{
    [Range(AgglomerativeConfig.Threshold.MinAllowed, AgglomerativeConfig.Threshold.MaxAllowed)]
    public double Threshold { get; set; } = AgglomerativeConfig.Threshold.Default;
}
