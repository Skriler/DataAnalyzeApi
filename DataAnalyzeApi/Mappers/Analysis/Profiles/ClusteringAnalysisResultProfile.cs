using AutoMapper;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering;
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
            )
            .ForMember(
                dest => dest.ObjectCoordinates,
                opt => opt.MapFrom(src => src.Clusters.SelectMany(c => c.Objects))
            );

        CreateMap<ClusterDto, Cluster>();

        CreateMap<DataObjectClusteringAnalysisDto, DataObject>()
            .ForMember(dest => dest.Values, opt => opt.Ignore());

        CreateMap<DataObjectClusteringAnalysisDto, DataObjectCoordinate>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(
                dest => dest.ObjectId,
                opt => opt.MapFrom(src => src.Id)
           );
    }
}
