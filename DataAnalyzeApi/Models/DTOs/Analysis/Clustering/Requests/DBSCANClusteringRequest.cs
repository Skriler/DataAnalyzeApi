using DataAnalyzeApi.Models.Config.Clustering;
using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Requests;

public record DBSCANClusteringRequest : BaseClusteringRequest
{
    [Range(DBSCANConfig.Epsilon.MinAllowed, DBSCANConfig.Epsilon.MaxAllowed)]
    public double Epsilon { get; init; } = DBSCANConfig.Epsilon.Default;

    [Range(DBSCANConfig.MinPoints.MinAllowed, DBSCANConfig.MinPoints.MaxAllowed)]
    public int MinPoints { get; init; } = DBSCANConfig.MinPoints.Default;
}
