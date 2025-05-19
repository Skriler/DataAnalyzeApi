using AutoFixture;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories.Models;

public class DatasetModelFactory
{
    private readonly Fixture fixture;
    private readonly DataObjectModelFactory dataObjectModelFactory;
    private readonly ParameterStateModelFactory parameterStateModelFactory;

    public DatasetModelFactory()
    {
        fixture = new Fixture();
        dataObjectModelFactory = new DataObjectModelFactory(fixture);
        parameterStateModelFactory = new ParameterStateModelFactory(fixture);
    }

    /// <summary>
    /// Creates a DatasetModel from raw values.
    /// </summary>
    public DatasetModel Create(List<RawDataObject> rawObjects)
    {
        var objectsModels = dataObjectModelFactory.CreateList(rawObjects);
        var parameterStates = parameterStateModelFactory.CreateList(objectsModels);

        return fixture.Build<DatasetModel>()
            .With(d => d.Parameters, parameterStates)
            .With(d => d.Objects, objectsModels)
            .Create();
    }

    /// <summary>
    /// Creates a DatasetModel with numeric and categorical values.
    /// </summary>
    public DatasetModel CreateNormalized(List<NormalizedDataObject> normalizedObjects)
    {
        var objectsModels = dataObjectModelFactory.CreateNormalizedList(normalizedObjects);
        var parameterStates = parameterStateModelFactory.CreateList(objectsModels);

        return fixture.Build<DatasetModel>()
            .With(d => d.Parameters, parameterStates)
            .With(d => d.Objects, objectsModels)
            .Create();
    }

    /// <summary>
    /// Creates a DataObjectModel with raw values.
    /// </summary>
    public DataObjectModel CreateDataObjectModel(RawDataObject rawObject) =>
        dataObjectModelFactory.Create(rawObject);

    /// <summary>
    /// Creates DataObjectModel list with numeric and categorical values.
    /// </summary>
    public DataObjectModel CreateNormalizedDataObjectModel(NormalizedDataObject normalizedObject) =>
        dataObjectModelFactory.CreateNormalized(normalizedObject);
}
