using AutoMapper;
using DataAnalyzeAPI.Models.DTOs.Create;
using DataAnalyzeAPI.Models.Entities;

namespace DataAnalyzeAPI.Mappers;

public class DatasetProfile : Profile
{
    public DatasetProfile()
    {
        CreateMap<DatasetCreateDto, Dataset>()
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
                opt => opt.MapFrom((src, dest) => MapObjects(src.Objects, dest.Parameters))
                );
    }

    private static List<Parameter> MapParameter(List<string> parameters)
    {
        return parameters.ConvertAll(p => new Parameter { Name = p });
    }

    private static List<DataObject> MapObjects(List<DataObjectCreateDTO> objects, List<Parameter> parameters)
    {
        return objects.ConvertAll(obj => new DataObject
        {
            Name = obj.Name,
            Values = MapParameterValues(obj.Values, parameters)
        });
    }

    private static List<ParameterValue> MapParameterValues(List<string> values, List<Parameter> parameters)
    {
        return values.Select((val, index) => new ParameterValue
        {
            Value = val,
            Parameter = parameters[index]
        }).ToList();
    }
}
