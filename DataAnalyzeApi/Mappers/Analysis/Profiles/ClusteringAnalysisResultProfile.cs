using AutoMapper;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;

namespace DataAnalyzeApi.Mappers.Analysis.Profiles;

/// <summary>
/// AutoMapper profile for Cluster Analysis operations (ClusteringAnalysisResultDto -> ClusteringAnalysisResult)
/// </summary>
public class ClusteringAnalysisResultProfile : Profile
{
    /// <summary>
    /// Configures mappings for Cluster Analysis using AutoMapper.
    /// </summary>
    public ClusteringAnalysisResultProfile()
    {
        CreateMap<ClusteringAnalysisResultDto, ClusteringAnalysisResult>()
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(_ => DateTime.UtcNow)
            );

        CreateMap<ClusterDto, Cluster>();

        CreateMap<DataObjectAnalysisDto, DataObject>()
            .ForMember(dest => dest.Values, opt => opt.Ignore());
    }
}
