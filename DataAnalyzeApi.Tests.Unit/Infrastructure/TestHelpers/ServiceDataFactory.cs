using AutoFixture;
using DataAnalyzeApi.Models.Domain.Clustering.KMeans;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;
using DataAnalyzeApi.Models.Enum;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;

public class ServiceDataFactory
{
    private readonly Fixture fixture;

    public ServiceDataFactory()
    {
        fixture = new Fixture();
    }

    /// <summary>
    /// Creates only numeric parameter values.
    /// </summary>
    public List<ParameterValueModel> CreateValueModelList(List<string> values)
    {
        var valueModels = new List<ParameterValueModel>();

        for (int i = 0; i < values.Count; ++i)
        {
            var parameterType = DetermineParameterType(values[i]);

            var parameterModel = fixture.Build<ParameterStateModel>()
                .With(s => s.Id, i)
                .With(s => s.Type, parameterType)
                .Create();

            valueModels.Add(fixture
                .Build<ParameterValueModel>()
                .With(p => p.Value, values[i])
                .With(p => p.ParameterId, i)
                .With(p => p.Parameter, parameterModel)
                .Create());
        }

        return valueModels;
    }

    /// <summary>
    /// Creates both numeric and categorical parameter values.
    /// </summary>
    public List<ParameterValueModel> CreateNormalizedValueModelList(List<double>? numerics, List<int[]>? categoricals)
    {
        var valueModels = new List<ParameterValueModel>();

        if (numerics != null)
        {
            valueModels.AddRange(CreateNormalizedNumericModelList(numerics));
        }

        if (categoricals != null)
        {
            valueModels.AddRange(CreateNormalizedCategoricalModelList(categoricals, numerics?.Count ?? 0));
        }

        return valueModels;
    }

    /// <summary>
    /// Creates only numeric parameter values.
    /// </summary>
    public List<ParameterValueModel> CreateNormalizedNumericModelList(List<double> numerics)
    {
        var valueModels = new List<ParameterValueModel>();

        for (int i = 0; i < numerics.Count; ++i)
        {
            valueModels.Add(fixture
                .Build<NormalizedNumericValueModel>()
                .With(p => p.NormalizedValue, numerics[i])
                .With(p => p.ParameterId, i)
                .Create());
        }

        return valueModels;
    }

    /// <summary>
    /// Creates only categorical parameter values with specified start ID.
    /// </summary>
    public List<ParameterValueModel> CreateNormalizedCategoricalModelList(List<int[]> categoricals, int startId = 0)
    {
        var valueModels = new List<ParameterValueModel>();

        for (int i = 0; i < categoricals.Count; ++i)
        {
            valueModels.Add(fixture
                .Build<NormalizedCategoricalValueModel>()
                .With(p => p.OneHotValues, categoricals[i])
                .With(p => p.ParameterId, startId + i)
                .Create());
        }

        return valueModels;
    }

    /// <summary>
    /// Creates a Centroid with the specified numeric and categorical values.
    /// </summary>
    public Centroid CreateCentroid(NormalizedDataObject centroid)
    {
        var valueModels = CreateNormalizedValueModelList(
            centroid.NumericValues,
            centroid.CategoricalValues);

        return fixture.Build<Centroid>()
            .With(obj => obj.Values, valueModels)
            .Create();
    }

    /// <summary>
    /// Creates a DataObjectModel with the specified numeric and categorical values.
    /// </summary>
    public DataObjectModel CreateNormalizedDataObjectModel(NormalizedDataObject normalizedObject, int id = 0)
    {
        var valueModels = CreateNormalizedValueModelList(
            normalizedObject.NumericValues,
            normalizedObject.CategoricalValues);

        return fixture.Build<DataObjectModel>()
            .With(obj => obj.Id, id)
            .With(obj => obj.Values, valueModels)
            .Create();
    }

    /// <summary>
    /// Creates a DataObjectModel with the specified numeric and categorical values.
    /// </summary>
    public DataObjectModel CreateDataObjectModel(RawDataObject rawObject, int id = 0)
    {
        var valueModels = CreateValueModelList(rawObject.Values);

        return fixture.Build<DataObjectModel>()
            .With(obj => obj.Id, id)
            .With(obj => obj.Values, valueModels)
            .Create();
    }

    /// <summary>
    /// Creates a DatasetModel with the specified numeric and categorical values.
    /// </summary>
    public DatasetModel CreateNormalizedDatasetModel(List<NormalizedDataObject> normalizedObjects)
    {
        var objectsModels = new List<DataObjectModel>();

        for (int i = 0; i < normalizedObjects.Count; ++i)
        {
            objectsModels.Add(CreateNormalizedDataObjectModel(normalizedObjects[i], i));
        }

        return fixture.Build<DatasetModel>()
            .With(d => d.Objects, objectsModels)
            .Create();
    }

    /// <summary>
    /// Creates a DatasetModel with the specified numeric and categorical values.
    /// </summary>
    public DatasetModel CreateDatasetModel(List<RawDataObject> rawObjects)
    {
        var objectsModels = new List<DataObjectModel>();

        for (int i = 0; i < rawObjects.Count; ++i)
        {
            objectsModels.Add(CreateDataObjectModel(rawObjects[i], i));
        }

        return fixture.Build<DatasetModel>()
            .With(d => d.Objects, objectsModels)
            .Create();
    }

    /// <summary>
    /// Determines the parameter type based on the value.
    /// </summary>
    private static ParameterType DetermineParameterType(string value) =>
        double.TryParse(value, out _) ? ParameterType.Numeric : ParameterType.Categorical;
}
