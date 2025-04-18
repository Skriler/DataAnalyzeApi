using DataAnalyzeAPI.Models.Config.Clustering;
using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;

public record DBSCANClusteringRequest : BaseClusteringRequest
{
    [Range(DBSCANConfig.Epsilon.MinAllowed, DBSCANConfig.Epsilon.MaxAllowed)]
    public double Epsilon { get; set; } = DBSCANConfig.Epsilon.Default;

    [Range(DBSCANConfig.MinPoints.MinAllowed, DBSCANConfig.MinPoints.MaxAllowed)]
    public int MinPoints { get; set; } = DBSCANConfig.MinPoints.Default;
}
