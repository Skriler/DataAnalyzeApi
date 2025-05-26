using DataAnalyzeApi.Models.Config.Clustering;
using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Requests;

public record KMeansClusteringRequest : BaseClusteringRequest
{
    [Range(KMeansConfig.MaxIterations.MinAllowed, KMeansConfig.MaxIterations.MaxAllowed)]
    public int MaxIterations { get; init; } = KMeansConfig.MaxIterations.Default;

    [Range(KMeansConfig.NumberOfClusters.MinAllowed, KMeansConfig.NumberOfClusters.MaxAllowed)]
    public int NumberOfClusters { get; init; } = KMeansConfig.NumberOfClusters.Default;
}
