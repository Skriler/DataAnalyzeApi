using AutoFixture;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings;
using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using DataAnalyzeApi.Tests.Common.Models.Analyse;

namespace DataAnalyzeApi.Tests.Common.Factories;

public class DtoTestDataFactory
{
    protected readonly Fixture fixture = new();

    /// <summary>
    /// Creates DataObjectCreateDto list from RawDataObject list.
    /// </summary>
    public List<DataObjectCreateDto> CreateDataObjectCreateDtoList(List<RawDataObject> rawObjects)
    {
        var objectCreateDtos = new List<DataObjectCreateDto>();

        foreach (var rawObject in rawObjects)
        {
            objectCreateDtos.Add(fixture
                .Build<DataObjectCreateDto>()
                .With(obj => obj.Values, rawObject.Values)
                .Create());
        }

        return objectCreateDtos;
    }

    /// <summary>
    /// Creates a DatasetCreateDto with RawDataObject list and specified parameters.
    /// </summary>
    public DatasetCreateDto CreateDatasetCreateDto(List<RawDataObject> rawObjects, List<string> rawParameters)
    {
        var objectCreateDtos = CreateDataObjectCreateDtoList(rawObjects);

        return fixture
            .Build<DatasetCreateDto>()
            .With(d => d.Objects, objectCreateDtos)
            .With(d => d.Parameters, rawParameters)
            .Create();
    }

    /// <summary>
    /// Creates ParameterSettingsDto list from raw parameter names.
    /// </summary>
    public List<ParameterSettingsDto> CreateParameterSettingsDtoList(List<string> rawParameters)
    {
        var parameterSettings = new List<ParameterSettingsDto>();

        for (int i = 0; i < rawParameters.Count; ++i)
        {
            parameterSettings.Add(fixture
                .Build<ParameterSettingsDto>()
                .With(s => s.ParameterId, i)
                .Create());
        }

        return parameterSettings;
    }
}
