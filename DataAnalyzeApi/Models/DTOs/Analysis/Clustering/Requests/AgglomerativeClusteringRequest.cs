using System.ComponentModel.DataAnnotations;
using DataAnalyzeApi.Models.Config.Clustering;

namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Requests;

public record AgglomerativeClusteringRequest : BaseClusteringRequest
{
    [Range(AgglomerativeConfig.Threshold.MinAllowed, AgglomerativeConfig.Threshold.MaxAllowed)]
    public double Threshold { get; init; } = AgglomerativeConfig.Threshold.Default;
}
