using AutoFixture;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories.Models;

public class DataObjectModelFactory
{
    private readonly Fixture fixture;
    private readonly ValueModelFactory valueModelFactory;

    public DataObjectModelFactory(Fixture fixture)
    {
        this.fixture = fixture;
        valueModelFactory = new ValueModelFactory(fixture);
    }

    /// <summary>
    /// Creates a DataObjectModel with raw values.
    /// </summary>
    public DataObjectModel Create(RawDataObject rawObject, int id = 0)
    {
        var valueModels = valueModelFactory.CreateList(rawObject.Values);

        return fixture.Build<DataObjectModel>()
            .With(obj => obj.Id, id)
            .With(obj => obj.Values, valueModels)
            .Create();
    }

    /// <summary>
    /// Creates a DataObjectModel list with raw values.
    /// </summary>
    public List<DataObjectModel> CreateList(List<RawDataObject> rawObjects) =>
        rawObjects
            .Select((obj, index) => Create(obj, index))
            .ToList();

    /// <summary>
    /// Creates a DataObjectModel with numeric and categorical values.
    /// </summary>
    public DataObjectModel CreateNormalized(NormalizedDataObject normalizedObject, int id = 0)
    {
        var valueModels = valueModelFactory.CreateNormalizedList(
            normalizedObject.NumericValues,
            normalizedObject.CategoricalValues);

        return fixture.Build<DataObjectModel>()
            .With(obj => obj.Id, id)
            .With(obj => obj.Values, valueModels)
            .Create();
    }

    /// <summary>
    /// Creates DataObjectModel list with numeric and categorical values.
    /// </summary>
    public List<DataObjectModel> CreateNormalizedList(List<NormalizedDataObject> normalizedObjects) =>
        normalizedObjects
            .Select((obj, index) => CreateNormalized(obj, index))
            .ToList();
}
