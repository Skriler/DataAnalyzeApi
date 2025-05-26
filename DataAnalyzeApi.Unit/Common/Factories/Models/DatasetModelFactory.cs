using AutoFixture;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Unit.Common.Models.Analyse;

namespace DataAnalyzeApi.Unit.Common.Factories.Models;

public class DatasetModelFactory
{
    private readonly Fixture fixture;
    private readonly DataObjectModelFactory objectModelFactory;
    private readonly ParameterStateModelFactory stateModelFactory;

    public DatasetModelFactory()
    {
        fixture = new Fixture();
        objectModelFactory = new DataObjectModelFactory(fixture);
        stateModelFactory = new ParameterStateModelFactory(fixture);
    }

    /// <summary>
    /// Creates a DatasetModel from RawDataObject list.
    /// </summary>
    public DatasetModel Create(List<RawDataObject> rawObjects)
    {
        var parameterStates = stateModelFactory.CreateList(rawObjects);
        var objectsModels = objectModelFactory.CreateList(rawObjects, parameterStates);

        return fixture.Build<DatasetModel>()
            .With(d => d.Parameters, parameterStates)
            .With(d => d.Objects, objectsModels)
            .Create();
    }

    /// <summary>
    /// Creates a DatasetModel with NormalizedDataObject list.
    /// </summary>
    public DatasetModel CreateNormalized(List<NormalizedDataObject> normalizedObjects)
    {
        var parameterStates = stateModelFactory.CreateList(normalizedObjects);
        var objectsModels = objectModelFactory.CreateNormalizedList(normalizedObjects, parameterStates);

        return fixture.Build<DatasetModel>()
            .With(d => d.Parameters, parameterStates)
            .With(d => d.Objects, objectsModels)
            .Create();
    }
}
