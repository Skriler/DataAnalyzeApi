using AutoFixture;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;

namespace DataAnalyzeApi.Unit.Common.Factories.Datasets.Models;

public class ParameterValueModelFactory
{
    private readonly Fixture fixture;

    public ParameterValueModelFactory()
    {
        fixture = new();
    }

    public ParameterValueModelFactory(Fixture fixture)
    {
        this.fixture = fixture;
    }

    /// <summary>
    /// Creates ParameterValueModel with specified id, value and optional ParameterValueModel.
    /// </summary>
    public ParameterValueModel Create(
        string value,
        int id = 0,
        ParameterStateModel? parameter = null)
    {
        var modelBuilder = fixture
            .Build<ParameterValueModel>()
            .With(val => val.Id, id)
            .With(val => val.Value, value);

        if (parameter != null)
        {
            modelBuilder = modelBuilder
                .With(val => val.ParameterId, parameter.Id)
                .With(val => val.Parameter, parameter);
        }
        else
        {
            modelBuilder = modelBuilder
                .With(val => val.ParameterId, id)
                .Without(val => val.Parameter);
        }

        return modelBuilder.Create();
    }

    /// <summary>
    /// Creates ParameterValueModel list from raw values and optional ParameterStateModel list.
    /// </summary>
    public List<ParameterValueModel> CreateList(
        List<string> values,
        List<ParameterStateModel>? parameters = null)
    {
        var valueModels = new List<ParameterValueModel>(values.Count);

        for (int i = 0; i < values.Count; ++i)
        {
            var valueModel = Create(values[i], i, parameters?.ElementAtOrDefault(i));

            valueModels.Add(valueModel);
        }

        return valueModels;
    }

    /// <summary>
    /// Creates NormalizedNumericValueModel with specified id, value and optional ParameterStateModel.
    /// </summary>
    public NormalizedNumericValueModel CreateNumeric(
        double value,
        int id = 0,
        ParameterStateModel? parameter = null)
    {
        var valueModel = Create(value.ToString(), id, parameter);

        return new NormalizedNumericValueModel(
            valueModel.Id,
            valueModel.Value,
            valueModel.ParameterId,
            valueModel.Parameter,
            value);
    }

    /// <summary>
    /// Creates ParameterValueModel list with numeric values and optional ParameterStateModel list.
    /// </summary>
    public List<ParameterValueModel> CreateNumericList(
        List<double> numerics,
        List<ParameterStateModel>? parameters = null)
    {
        var valueModels = new List<ParameterValueModel>();

        for (int i = 0; i < numerics.Count; ++i)
        {
            var valueModel = CreateNumeric(
                numerics[i],
                i,
                parameters?.ElementAtOrDefault(i));

            valueModels.Add(valueModel);
        }

        return valueModels;
    }

    /// <summary>
    /// Creates NormalizedCategoricalValueModel with specified id, one-hot values and optional ParameterStateModel.
    /// </summary>
    public NormalizedCategoricalValueModel CreateCategorical(
        int[] oneHotValues,
        int id = 0,
        ParameterStateModel? parameter = null)
    {
        var valueModel = Create(fixture.Create<string>(), id, parameter);

        return new NormalizedCategoricalValueModel(
            valueModel.Id,
            valueModel.Value,
            valueModel.ParameterId,
            valueModel.Parameter,
            oneHotValues);
    }

    /// <summary>
    /// Creates ParameterValueModel list with categorical values,
    /// optional start ID and optional ParameterStateModel list.
    /// </summary>
    public List<ParameterValueModel> CreateCategoricalList(
        List<int[]> categoricals,
        int startId = 0,
        List<ParameterStateModel>? parameters = null)
    {
        var valueModels = new List<ParameterValueModel>();

        for (int i = 0; i < categoricals.Count; ++i)
        {
            var actualId = startId + i;
            var valueModel = CreateCategorical(
                categoricals[i],
                actualId,
                parameters?.ElementAtOrDefault(actualId));

            valueModels.Add(valueModel);
        }

        return valueModels;
    }

    /// <summary>
    /// Creates ParameterValueModel list with numeric, categorical values
    /// and optional ParameterStateModel list
    /// </summary>
    public List<ParameterValueModel> CreateNormalizedList(
        List<double>? numerics,
        List<int[]>? categoricals,
        List<ParameterStateModel>? parameters = null)
    {
        var valueModels = new List<ParameterValueModel>();

        if (numerics != null)
        {
            var numericValueModels = CreateNumericList(
                numerics,
                parameters);

            valueModels.AddRange(numericValueModels);
        }

        if (categoricals != null)
        {
            var categoricalValueModels = CreateCategoricalList(
                categoricals,
                numerics?.Count ?? 0,
                parameters);

            valueModels.AddRange(categoricalValueModels);
        }

        return valueModels;
    }
}
