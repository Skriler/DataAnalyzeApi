using AutoFixture;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Enum;
using DataAnalyzeApi.Tests.Common.Models.Analyse;

namespace DataAnalyzeApi.Tests.Common.Factories.Models;

public class ParameterStateModelFactory(Fixture fixture)
{
    private readonly Fixture fixture = fixture;

    /// <summary>
    /// Creates ParameterStateModel list from RawDataObject value's types.
    /// </summary>
    public List<ParameterStateModel> CreateList(List<RawDataObject> rawObjects)
    {
        var parameterCount = rawObjects.FirstOrDefault()?.Values.Count ?? 0;
        var parameterStateModels = new List<ParameterStateModel>(parameterCount);

        for (int i = 0; i < parameterCount; ++i)
        {
            var values = rawObjects.ConvertAll(obj => obj.Values[i]);
            var type = DetermineParameterType(values);

            parameterStateModels.Add(CreateParameterState(i, type));
        }

        return parameterStateModels;
    }

    /// <summary>
    /// Creates ParameterStateModel list from NormalizedDataObject value's types.
    /// </summary>
    public List<ParameterStateModel> CreateList(List<NormalizedDataObject> normalizedObjects)
    {
        var numericParameterCount = normalizedObjects[0].NumericValues?.Count ?? 0;
        var categoricalParameterCount = normalizedObjects[0].CategoricalValues?.Count ?? 0;
        var parameterStateModels = new List<ParameterStateModel>(numericParameterCount + categoricalParameterCount);

        for (int i = 0; i < numericParameterCount; ++i)
        {
            parameterStateModels.Add(CreateParameterState(i, ParameterType.Numeric));
        }

        for (int i = 0; i < categoricalParameterCount; ++i)
        {
            parameterStateModels.Add(CreateParameterState(i, ParameterType.Categorical));
        }

        return parameterStateModels;
    }

    /// <summary>
    /// Creates ParameterStateModel with specified id and type.
    /// </summary>
    private ParameterStateModel CreateParameterState(int id, ParameterType type) =>
        fixture.Build<ParameterStateModel>()
            .With(p => p.Id, id)
            .With(p => p.Type, type)
            .With(p => p.IsActive, true)
            .With(p => p.Weight, 1)
            .Create();

    /// <summary>
    /// Determines the parameter type based on the values.
    /// </summary>
    private static ParameterType DetermineParameterType(List<string> values)
    {
        if (values.All(string.IsNullOrWhiteSpace))
        {
            return ParameterType.Categorical;
        }

        if (values.Any(val => double.TryParse(val, out _)))
        {
            return ParameterType.Numeric;
        }

        return ParameterType.Categorical;
    }
}
