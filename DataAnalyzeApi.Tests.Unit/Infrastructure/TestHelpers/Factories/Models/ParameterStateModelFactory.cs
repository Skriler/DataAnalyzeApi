using AutoFixture;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Enum;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories.Models;

public class ParameterStateModelFactory(Fixture fixture)
{
    private readonly Fixture fixture = fixture;

    /// <summary>
    /// Creates ParameterStateModel list from DataObjectModel value's types.
    /// </summary>
    public List<ParameterStateModel> CreateList(List<DataObjectModel> valueModels)
    {
        var parameterStateModels = new List<ParameterStateModel>();

        for (int i = 0; i < valueModels[0].Values.Count; ++i)
        {
            var values = valueModels.ConvertAll(obj => obj.Values[i].Value);
            var type = DetermineParameterType(values);

            parameterStateModels.Add(fixture
                .Build<ParameterStateModel>()
                .With(s => s.Id, i)
                .With(s => s.Type, type)
                .Create());
        }

        return parameterStateModels;
    }

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
