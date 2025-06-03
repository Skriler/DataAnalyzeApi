using DataAnalyzeApi.Models.DTOs.Dataset.Read;
using DataAnalyzeApi.Models.Entities;
using AutoMapper;

namespace DataAnalyzeApi.Mappers.Profiles;

/// <summary>
/// AutoMapper profile for Dataset read operations (Dataset -> DatasetDto)
/// </summary>
public class DatasetReadProfile : Profile
{
    /// <summary>
    /// Configures mappings for reading Dataset data using AutoMapper.
    /// </summary>
    public DatasetReadProfile()
    {
        CreateMap<Dataset, DatasetDto>();
        CreateMap<Parameter, ParameterDto>();
        CreateMap<DataObject, DataObjectDto>();
        CreateMap<ParameterValue, ParameterValueDto>();
    }
}
