using AutoMapper;
using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Enum;

namespace DataAnalyzeApi.Mappers.Entities.Profiles;

/// <summary>
/// AutoMapper profile for Dataset creation operations (Dataset <-> DatasetCreateDto)
/// </summary>
public class DatasetCreateProfile : Profile
{
    /// <summary>
    /// Configures mappings between CreateDTOs and entities using AutoMapper.
    /// </summary>
    public DatasetCreateProfile()
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
            .AfterMap((_, dest) =>
            {
                AfterMapParameters(dest);
                AfterMapParameterValues(dest.Objects, dest.Parameters);
            });

        CreateMap<Dataset, DatasetCreateDto>()
            .ForMember(
                 dest => dest.Parameters,
                 opt => opt.MapFrom(src => src.Parameters.ConvertAll(p => p.Name))
                 )
            .ForMember(
                 dest => dest.Objects,
                 opt => opt.MapFrom(src => src.Objects)
                );

        CreateMap<DataObject, DataObjectCreateDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom(src => src.Values.ConvertAll(val => val.Value))
                );
    }

    /// <summary>
    /// Maps parameter names to Parameter entities.
    /// </summary>
    private static List<Parameter> MapParameter(List<string> parameters)
    {
        return parameters.ConvertAll(p => new Parameter { Name = p });
    }

    /// <summary>
    /// Maps DataObjectCreateDto models to DataObject entities.
    /// </summary>
    private static List<DataObject> MapObjects(List<DataObjectCreateDto> objects)
    {
        return objects.ConvertAll(obj => new DataObject
        {
            Name = obj.Name,
            Values = obj.Values.ConvertAll(val => new ParameterValue { Value = val })
        });
    }

    /// <summary>
    /// Assigns dataset and determines type for each parameter.
    /// </summary>
    private static void AfterMapParameters(Dataset dataset)
    {
        for (int i = 0; i < dataset.Parameters.Count; ++i)
        {
            var values = dataset.Objects.ConvertAll(obj => obj.Values[i].Value);

            dataset.Parameters[i].Type = DetermineParameterType(values);
            dataset.Parameters[i].Dataset = dataset;
        }
    }

    /// <summary>
    /// Assigns parameters to each value in all data objects.
    /// </summary>
    private static void AfterMapParameterValues(List<DataObject> objects, List<Parameter> parameters)
    {
        foreach (var obj in objects)
        {
            for (int i = 0; i < obj.Values.Count; ++i)
            {
                obj.Values[i].Parameter = parameters[i];
            }
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
