using AutoFixture;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Tests.Common.Models;

namespace DataAnalyzeApi.Tests.Common.Factories.Models;

public class DataObjectModelFactory
{
    private readonly Fixture fixture;
    private readonly ParameterValueModelFactory valueModelFactory;

    public DataObjectModelFactory()
    {
        fixture = new();
        valueModelFactory = new ParameterValueModelFactory(fixture);
    }

    public DataObjectModelFactory(Fixture fixture)
    {
        this.fixture = fixture;
        valueModelFactory = new ParameterValueModelFactory(fixture);
    }

    /// <summary>
    /// Creates a DataObjectModel with raw values and associated ParameterStateModel list.
    /// </summary>
    public DataObjectModel Create(
        RawDataObject rawObject,
        int id = 0,
        List<ParameterStateModel>? parameters = null)
    {
        var valueModels = valueModelFactory.CreateList(rawObject.Values, parameters);

        return fixture.Build<DataObjectModel>()
            .With(obj => obj.Id, id)
            .With(obj => obj.Values, valueModels)
            .Create();
    }

    /// <summary>
    /// Creates a DataObjectModel list with raw values with associated ParameterStateModel list.
    /// </summary>
    public List<DataObjectModel> CreateList(
        List<RawDataObject> rawObjects,
        List<ParameterStateModel>? parameters = null) =>
        rawObjects
            .Select((obj, index) => Create(obj, index, parameters))
            .ToList();

    /// <summary>
    /// Creates a DataObjectModel with numeric and categorical values and associated ParameterStateModel list.
    /// </summary>
    public DataObjectModel CreateNormalized(
        NormalizedDataObject normalizedObject,
        int id = 0,
        List<ParameterStateModel>? parameters = null)
    {
        var valueModels = valueModelFactory.CreateNormalizedList(
            normalizedObject.NumericValues,
            normalizedObject.CategoricalValues,
            parameters);

        return fixture.Build<DataObjectModel>()
            .With(obj => obj.Id, id)
            .With(obj => obj.Values, valueModels)
            .Create();
    }

    /// <summary>
    /// Creates DataObjectModel list with numeric and categorical values and associated ParameterStateModel list.
    /// </summary>
    public List<DataObjectModel> CreateNormalizedList(
        List<NormalizedDataObject> normalizedObjects,
        List<ParameterStateModel>? parameters = null) =>
        normalizedObjects
            .Select((obj, index) => CreateNormalized(obj, index, parameters))
            .ToList();
}
