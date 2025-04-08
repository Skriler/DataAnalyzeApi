using DataAnalyzeAPI.Models.Config.Clustering;
using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;

public class KMeansClusteringRequest : BaseClusteringRequest
{
    [Range(KMeansConfig.MaxIterations.MinAllowed, KMeansConfig.MaxIterations.MaxAllowed)]
    public int MaxIterations { get; set; } = KMeansConfig.MaxIterations.Default;

    [Range(KMeansConfig.NumberOfClusters.MinAllowed, KMeansConfig.NumberOfClusters.MaxAllowed)]
    public int NumberOfClusters { get; set; } = KMeansConfig.NumberOfClusters.Default;
}
