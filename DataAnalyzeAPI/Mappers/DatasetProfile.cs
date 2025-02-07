using AutoMapper;
using DataAnalyzeAPI.Models.DTOs;
using DataAnalyzeAPI.Models.Entities;

namespace DataAnalyzeAPI.Mappers;

public class DatasetProfile : Profile
{
    public DatasetProfile()
    {
        CreateMap<DatasetCreateDto, Dataset>()
            .ForMember(
                dest => dest.Parameters,
                opt => opt.MapFrom(src => src.Parameters.Select(
                    p => new Parameter { Name = p }
                ))
            )
            .ForMember(
                dest => dest.Objects,
                opt => opt.MapFrom(src => src.Objects)
            )
            .ForMember(
                dest => dest.CreatedAt, 
                opt => opt.MapFrom(src => DateTime.UtcNow)
            );
    }
}
