using AutoMapper;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;

namespace DataAnalyzeApi.Mappers.Analysis.Profiles;

/// <summary>
/// AutoMapper profile for Similarity Analysis operations (SimilarityAnalysisResultDto -> SimilarityAnalysisResult)
/// </summary>
public class SimilarityAnalysisResultProfile : Profile
{
    /// <summary>
    /// Configures mappings for Similarity Analysis using AutoMapper.
    /// </summary>
    public SimilarityAnalysisResultProfile()
    {
        CreateMap<SimilarityAnalysisResultDto, SimilarityAnalysisResult>()
            .ForMember(
                dest => dest.SimilarityPairs,
                opt => opt.MapFrom(src => src.Similarities)
            )
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(_ => DateTime.UtcNow)
            );

        CreateMap<SimilarityPairDto, SimilarityPair>()
            .ForMember(dest => dest.ObjectAId, opt => opt.MapFrom(src => src.ObjectA.Id))
            .ForMember(dest => dest.ObjectBId, opt => opt.MapFrom(src => src.ObjectB.Id));

        CreateMap<DataObjectAnalysisDto, DataObject>()
            .ForMember(dest => dest.Values, opt => opt.Ignore());
    }
}
