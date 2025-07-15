using System.ComponentModel.DataAnnotations;
using DataAnalyzeApi.Models.Config.Clustering;

namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Requests;

public record KMeansClusterRequest : BaseClusteringRequest
{
    [Range(KMeansConfig.MaxIterations.MinAllowed, KMeansConfig.MaxIterations.MaxAllowed)]
    public int MaxIterations { get; init; } = KMeansConfig.MaxIterations.Default;

    [Range(KMeansConfig.NumberOfClusters.MinAllowed, KMeansConfig.NumberOfClusters.MaxAllowed)]
    public int NumberOfClusters { get; init; } = KMeansConfig.NumberOfClusters.Default;
}
