using AutoMapper;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;

namespace DataAnalyzeApi.Mappers.Analysis.Profiles;

/// <summary>
/// AutoMapper profile for Cluster Analysis operations (ClusterAnalysisResult <-> ClusterAnalysisResultDto)
/// </summary>
public class ClusterAnalysisResultProfile : Profile
{
    /// <summary>
    /// Configures mappings for Cluster Analysis using AutoMapper.
    /// </summary>
    public ClusterAnalysisResultProfile()
    {
        CreateMap<ClusterAnalysisResult, ClusterAnalysisResultDto>();

        CreateMap<ClusterAnalysisResultDto, ClusterAnalysisResult>();

        CreateMap<Cluster, ClusterDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => $"Cluster {src.Id}")
            );

        CreateMap<ClusterDto, Cluster>();

        CreateMap<DataObject, DataObjectAnalysisDto>()
            .ForMember(dest => dest.ParameterValues, opt => opt.Ignore())
            .AfterMap((src, dest) => dest.ParameterValues = null!);

        CreateMap<DataObjectAnalysisDto, DataObject>()
            .ForMember(dest => dest.Values, opt => opt.Ignore());
    }
}
