using AutoFixture;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories.Models;

public class ValueModelFactory(Fixture fixture)
{
    private readonly Fixture fixture = fixture;

    /// <summary>
    /// Creates ParameterValueModel list from raw values.
    /// </summary>
    public List<ParameterValueModel> CreateList(List<string> values)
    {
        var valueModels = new List<ParameterValueModel>();

        for (int i = 0; i < values.Count; ++i)
        {
            valueModels.Add(fixture
                .Build<ParameterValueModel>()
                .With(val => val.Id, i)
                .With(val => val.Value, values[i])
                .With(val => val.ParameterId, i)
                .Without(val => val.Parameter)
                .Create());
        }

        return valueModels;
    }

    /// <summary>
    /// Creates ParameterValueModel list with numeric and categorical values.
    /// </summary>
    public List<ParameterValueModel> CreateNormalizedList(List<double>? numerics, List<int[]>? categoricals)
    {
        var valueModels = new List<ParameterValueModel>();

        if (numerics != null)
        {
            valueModels.AddRange(CreateNormalizedNumericList(numerics));
        }

        if (categoricals != null)
        {
            valueModels.AddRange(CreateNormalizedCategoricalList(categoricals, numerics?.Count ?? 0));
        }

        return valueModels;
    }

    /// <summary>
    /// Creates ParameterValueModel list with numeric values.
    /// </summary>
    private List<ParameterValueModel> CreateNormalizedNumericList(List<double> numerics)
    {
        var valueModels = new List<ParameterValueModel>();

        for (int i = 0; i < numerics.Count; ++i)
        {
            valueModels.Add(fixture
                .Build<NormalizedNumericValueModel>()
                .With(val => val.Id, i)
                .With(val => val.Value, numerics[i].ToString())
                .With(val => val.NormalizedValue, numerics[i])
                .With(val => val.ParameterId, i)
                .Without(val => val.Parameter)
                .Create());
        }

        return valueModels;
    }

    /// <summary>
    /// Creates ParameterValueModel list with categorical values and optional start ID.
    /// </summary>
    private List<ParameterValueModel> CreateNormalizedCategoricalList(List<int[]> categoricals, int startId = 0)
    {
        var valueModels = new List<ParameterValueModel>();

        for (int i = 0; i < categoricals.Count; ++i)
        {
            valueModels.Add(fixture
                .Build<NormalizedCategoricalValueModel>()
                .With(val => val.Id, i)
                .With(val => val.Value, fixture.Create<string>())
                .With(val => val.OneHotValues, categoricals[i])
                .With(val => val.ParameterId, startId + i)
                .Without(val => val.Parameter)
                .Create());
        }

        return valueModels;
    }
}
