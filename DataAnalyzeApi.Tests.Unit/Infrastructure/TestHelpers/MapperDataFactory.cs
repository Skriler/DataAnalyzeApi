using AutoFixture;
using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Enum;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;

public class MapperDataFactory
{
    private readonly Fixture fixture;

    public MapperDataFactory()
    {
        fixture = new Fixture();
    }

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
    /// Creates a DatasetCreateDto with specified parameters and object values.
    /// </summary>
    public DatasetCreateDto CreateDatasetCreateDto(List<RawDataObject> rawObjects)
    {
        var objectCreateDtos = CreateDataObjectCreateDtoList(rawObjects);

        return fixture
            .Build<DatasetCreateDto>()
            .With(d => d.Objects, objectCreateDtos)
            .Create();
    }

    public List<Parameter> CreateParameterList(List<string> rawParameters)
    {
        var parameterEntities = new List<Parameter>();

        foreach (var rawParameter in rawParameters)
        {
            parameterEntities.Add(fixture
                .Build<Parameter>()
                .With(obj => obj.Name, rawParameter)
                .Create());
        }

        return parameterEntities;
    }

    public List<ParameterValue> CreateParameterValueList(RawDataObject rawObjects)
    {
        var valueEntities = new List<ParameterValue>();

        foreach (var value in rawObjects.Values)
        {
            valueEntities.Add(fixture
                .Build<ParameterValue>()
                .With(obj => obj.Value, value)
                .Create());
        }

        return valueEntities;
    }

    public List<DataObject> CreateDataObjectList(List<RawDataObject> rawObjects)
    {
        var objectCreateDtos = new List<DataObject>();

        foreach (var rawObject in rawObjects)
        {
            var parameterValues = CreateParameterValueList(rawObject);

            objectCreateDtos.Add(fixture
                .Build<DataObject>()
                .With(obj => obj.Values, parameterValues)
                .Create());
        }

        return objectCreateDtos;
    }

    /// <summary>
    /// Creates a Dataset with specified parameters and objects.
    /// </summary>
    public Dataset CreateDatasetEntity(List<RawDataObject> rawObjects, List<string> rawParameters)
    {
        var dataObjectEntities = CreateDataObjectList(rawObjects);
        var parameterEntities = CreateParameterList(rawParameters);

        return fixture
            .Build<Dataset>()
            .With(d => d.Objects, dataObjectEntities)
            .With(d => d.Parameters, parameterEntities)
            .Create();
    }

    /// <summary>
    /// Determines the parameter type based on the value.
    /// </summary>
    private static ParameterType DetermineParameterType(string value) =>
        double.TryParse(value, out _) ? ParameterType.Numeric : ParameterType.Categorical;
}
