using AutoFixture;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Tests.Common.Models.Analyse;

namespace DataAnalyzeApi.Tests.Common.Factories;

public class EntityTestDataFactory
{
    protected readonly Fixture fixture = new();

    /// <summary>
    /// Creates Parameter list from raw parameter names.
    /// </summary>
    public List<Parameter> CreateParameterList(List<string> rawParameters)
    {
        var parameterEntities = new List<Parameter>();

        for (int i = 0; i < rawParameters.Count; ++i)
        {
            parameterEntities.Add(fixture
               .Build<Parameter>()
               .With(p => p.Id, i)
               .With(p => p.Name, rawParameters[i])
               .Without(p => p.Dataset)
               .Create());
        }

        return parameterEntities;
    }

    /// <summary>
    /// Creates ParameterValue list from raw values.
    /// </summary>
    public List<ParameterValue> CreateParameterValueList(List<string> values)
    {
        var valueEntities = new List<ParameterValue>();

        for (int i = 0; i < values.Count; ++i)
        {
            valueEntities.Add(fixture
                .Build<ParameterValue>()
                .With(val => val.Id, i)
                .With(val => val.Value, values[i])
                .With(val => val.ParameterId, i)
                .Without(val => val.Parameter)
                .Without(val => val.Object)
                .Create());
        }

        return valueEntities;
    }

    /// <summary>
    /// Creates DataObject list from RawDataObject list.
    /// </summary>
    public List<DataObject> CreateDataObjectList(List<RawDataObject> rawObjects)
    {
        var objectCreateDtos = new List<DataObject>();

        for (int i = 0; i < rawObjects.Count; ++i)
        {
            var parameterValues = CreateParameterValueList(rawObjects[i].Values);

            objectCreateDtos.Add(fixture
                .Build<DataObject>()
                .With(obj => obj.Id, i)
                .With(obj => obj.Values, parameterValues)
                .Without(p => p.Dataset)
                .Create());
        }

        return objectCreateDtos;
    }

    /// <summary>
    /// Creates a Dataset with RawDataObject list and specified parameters.
    /// </summary>
    public Dataset CreateDataset(List<RawDataObject> rawObjects, List<string> rawParameters)
    {
        var dataObjectEntities = CreateDataObjectList(rawObjects);
        var parameterEntities = CreateParameterList(rawParameters);

        return fixture
            .Build<Dataset>()
            .With(d => d.Objects, dataObjectEntities)
            .With(d => d.Parameters, parameterEntities)
            .Create();
    }
}
