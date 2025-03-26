using AutoMapper;
using DataAnalyzeAPI.Models.DTOs.Dataset.Analysis;
using DataAnalyzeAPI.Models.DTOs.Dataset.Create;
using DataAnalyzeAPI.Models.Entities;
using DataAnalyzeAPI.Models.Enum;

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
                opt => opt.MapFrom(src => MapObjects(src.Objects))
                )
            .AfterMap((_, dest) => {
                MapParameterValues(dest.Objects, dest.Parameters);
                SetParameterTypes(dest.Objects, dest.Parameters);
            });

        CreateMap<Dataset, DatasetCreateDto>()
            .ForCtorParam("Parameters", opt => opt.MapFrom(src => src.Parameters.ConvertAll(p => p.Name)))
            .ForCtorParam("Objects", opt => opt.MapFrom(src => src.Objects));

        CreateMap<DataObject, DataObjectCreateDto>()
            .ForCtorParam("Values", opt => opt.MapFrom(src => src.Values.ConvertAll(val => val.Value ?? "")));
    }

    private static List<Parameter> MapParameter(List<string> parameters)
    {
        return parameters.ConvertAll(p => new Parameter { Name = p });
    }

    private static List<DataObject> MapObjects(List<DataObjectCreateDto> objects)
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

    /// <summary>
    /// Sets the type to each parameter based on the values.
    /// </summary>
    private static void SetParameterTypes(List<DataObject> objects, List<Parameter> parameters)
    {
        for (int i = 0; i < parameters.Count; ++i)
        {
            var values = objects
                .ConvertAll(obj => obj.Values[i].Value);

            parameters[i].Type = DetermineParameterType(values);
        }
    }

    /// <summary>
    /// Determines the parameter type based on the values.
    /// </summary>
    private static ParameterType DetermineParameterType(List<string> values)
    {
        if (values.All(string.IsNullOrWhiteSpace))
        {
            return ParameterType.Categorical;
        }

        if (values.Any(val => double.TryParse(val, out _)))
        {
            return ParameterType.Numeric;
        }

        return ParameterType.Categorical;
    }
}
