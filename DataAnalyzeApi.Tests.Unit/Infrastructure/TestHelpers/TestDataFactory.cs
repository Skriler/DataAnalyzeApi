using AutoFixture;
using DataAnalyzeApi.Models.Domain.Clustering.KMeans;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;
using DataAnalyzeApi.Models.Enum;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;

public class TestDataFactory
{
    private readonly Fixture fixture;

    public TestDataFactory()
    {
        fixture = new Fixture();
    }

    /// <summary>
    /// Creates only numeric parameter values.
    /// </summary>
    public List<ParameterValueModel> CreateValueModels(List<string> values)
    {
        var result = new List<ParameterValueModel>();

        for (int i = 0; i < values.Count; ++i)
        {
            var parameterType = DetermineParameterType(values[i]);

            var parameter = fixture.Build<ParameterStateModel>()
                .With(s => s.Id, i)
                .With(s => s.Type, parameterType)
                .Create();

            result.Add(fixture
                .Build<ParameterValueModel>()
                .With(p => p.Value, values[i])
                .With(p => p.ParameterId, i)
                .With(p => p.Parameter, parameter)
                .Create());
        }

        return result;
    }

    /// <summary>
    /// Creates both numeric and categorical parameter values.
    /// </summary>
    public List<ParameterValueModel> CreateNormalizedValueModels(List<double>? numerics, List<int[]>? categoricals)
    {
        var result = new List<ParameterValueModel>();

        if (numerics != null)
        {
            result.AddRange(CreateNormalizedNumericModel(numerics));
        }

        if (categoricals != null)
        {
            result.AddRange(CreateNormalizedCategoricalModel(categoricals, numerics?.Count ?? 0));
        }

        return result;
    }

    /// <summary>
    /// Creates only numeric parameter values.
    /// </summary>
    public List<ParameterValueModel> CreateNormalizedNumericModel(List<double> numerics)
    {
        var result = new List<ParameterValueModel>();

        for (int i = 0; i < numerics.Count; ++i)
        {
            result.Add(fixture
                .Build<NormalizedNumericValueModel>()
                .With(p => p.NormalizedValue, numerics[i])
                .With(p => p.ParameterId, i)
                .Create());
        }

        return result;
    }

    /// <summary>
    /// Creates only categorical parameter values with specified start ID.
    /// </summary>
    public List<ParameterValueModel> CreateNormalizedCategoricalModel(List<int[]> categoricals, int startId = 0)
    {
        var result = new List<ParameterValueModel>();

        for (int i = 0; i < categoricals.Count; ++i)
        {
            result.Add(fixture
                .Build<NormalizedCategoricalValueModel>()
                .With(p => p.OneHotValues, categoricals[i])
                .With(p => p.ParameterId, startId + i)
                .Create());
        }

        return result;
    }

    /// <summary>
    /// Creates a Centroid with the specified numeric and categorical values.
    /// </summary>
    public Centroid CreateCentroid(NormalizedDataObject centroid)
    {
        var valueModels = CreateNormalizedValueModels(
            centroid.NumericValues,
            centroid.CategoricalValues);

        return fixture.Build<Centroid>()
            .With(obj => obj.Values, valueModels)
            .Create();
    }

    /// <summary>
    /// Creates a DataObjectModel with the specified numeric and categorical values.
    /// </summary>
    public DataObjectModel CreateNormalizedDataObjectModel(NormalizedDataObject dataObject, int id = 0)
    {
        var valueModels = CreateNormalizedValueModels(
            dataObject.NumericValues,
            dataObject.CategoricalValues);

        return fixture.Build<DataObjectModel>()
            .With(obj => obj.Id, id)
            .With(obj => obj.Values, valueModels)
            .Create();
    }

    /// <summary>
    /// Creates a DataObjectModel with the specified numeric and categorical values.
    /// </summary>
    public DataObjectModel CreateDataObjectModel(RawDataObject dataObject, int id = 0)
    {
        var valueModels = CreateValueModels(dataObject.Values);

        return fixture.Build<DataObjectModel>()
            .With(obj => obj.Id, id)
            .With(obj => obj.Values, valueModels)
            .Create();
    }

    /// <summary>
    /// Creates a DatasetModel with the specified numeric and categorical values.
    /// </summary>
    public DatasetModel CreateNormalizedDatasetModel(List<NormalizedDataObject> dataObjects)
    {
        var dataObjectsModel = new List<DataObjectModel>();

        for (int i = 0; i < dataObjects.Count; ++i)
        {
            dataObjectsModel.Add(CreateNormalizedDataObjectModel(dataObjects[i], i));
        }

        return fixture.Build<DatasetModel>()
            .With(d => d.Objects, dataObjectsModel)
            .Create();
    }

    /// <summary>
    /// Creates a DatasetModel with the specified numeric and categorical values.
    /// </summary>
    public DatasetModel CreateDatasetModel(List<RawDataObject> dataObjects)
    {
        var dataObjectsModel = new List<DataObjectModel>();

        for (int i = 0; i < dataObjects.Count; ++i)
        {
            dataObjectsModel.Add(CreateDataObjectModel(dataObjects[i], i));
        }

        return fixture.Build<DatasetModel>()
            .With(d => d.Objects, dataObjectsModel)
            .Create();
    }

    /// <summary>
    /// Determines the parameter type based on the value.
    /// </summary>
    private static ParameterType DetermineParameterType(string value) =>
        double.TryParse(value, out _) ? ParameterType.Numeric : ParameterType.Categorical;
}
