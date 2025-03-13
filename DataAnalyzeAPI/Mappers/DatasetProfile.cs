using AutoMapper;
using DataAnalyzeAPI.Models.DTOs.Create;
using DataAnalyzeAPI.Models.DTOs.Dataset;
using DataAnalyzeAPI.Models.Entities;

namespace DataAnalyzeAPI.Mappers;

public class DatasetProfile : Profile
{
    public DatasetProfile()
    {
        CreateMap<DatasetDto, Dataset>()
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(_ => DateTime.UtcNow)
                )
            .ForMember(
                dest => dest.Parameters,
                opt => opt.MapFrom(src => MapParameter(src.Parameters))
                )
            .ForMember(
                dest => dest.Objects,
                opt => opt.MapFrom(src => MapObjects(src.Objects))
                )
            .AfterMap((_, dest) => MapParameterValues(dest.Objects, dest.Parameters));

        CreateMap<Dataset, DatasetDto>()
            .ForCtorParam("Parameters", opt => opt.MapFrom(src => src.Parameters.ConvertAll(p => p.Name)))
            .ForCtorParam("Objects", opt => opt.MapFrom(src => src.Objects));

        CreateMap<DataObject, DataObjectDTO>()
            .ForCtorParam("Values", opt => opt.MapFrom(src => src.Values.ConvertAll(val => val.Value ?? "")));
    }

    private static List<Parameter> MapParameter(List<string> parameters)
    {
        return parameters.ConvertAll(p => new Parameter { Name = p });
    }

    private static List<DataObject> MapObjects(List<DataObjectDTO> objects)
    {
        return objects.ConvertAll(obj => new DataObject
        {
            Name = obj.Name,
            Values = obj.Values.ConvertAll(val => new ParameterValue { Value = val })
        });
    }

    private static void MapParameterValues(List<DataObject> objects, List<Parameter> parameters)
    {
        foreach (var obj in objects)
        {
            for (int i = 0; i < obj.Values.Count; ++i)
            {
                var val = obj.Values[i];
                val.Parameter = parameters[i];
            }
        }
    }
}
