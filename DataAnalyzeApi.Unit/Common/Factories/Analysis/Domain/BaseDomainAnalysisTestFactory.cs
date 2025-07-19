using AutoFixture;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.DimensionalityReduction;

namespace DataAnalyzeApi.Unit.Common.Factories.Analysis.Domain;

public abstract class BaseDomainAnalysisTestFactory
{
    protected readonly Fixture fixture = new();

    /// <summary>
    /// Creates a DataObjectModel with test data.
    /// </summary>
    public DataObjectModel CreateDataObjectModel()
    {
        return fixture.Build<DataObjectModel>()
            .With(d => d.Id, fixture.Create<long>())
            .With(d => d.Name, fixture.Create<string>()[..10])
            .With(d => d.Values, CreateParameterValueModelList(fixture.Create<int>() % 5 + 1))
            .Create();
    }

    /// <summary>
    /// Creates DataObjectModel list.
    /// </summary>
    public List<DataObjectModel> CreateDataObjectModelList(int count) =>
        Enumerable.Range(0, count)
            .Select(_ => CreateDataObjectModel())
            .ToList();

    /// <summary>
    /// Creates a DataObjectCoordinateModel with test data.
    /// </summary>
    public DataObjectCoordinateModel CreateDataObjectCoordinateModel(long objectId)
    {
        return new DataObjectCoordinateModel(
            objectId,
            fixture.Create<double>(),
            fixture.Create<double>());
    }

    /// <summary>
    /// Creates DataObjectCoordinateModel list based on data objects.
    /// </summary>
    public List<DataObjectCoordinateModel> CreateDataObjectCoordinateModelList(List<DataObjectModel> dataObjects) =>
        dataObjects
            .Select(obj => CreateDataObjectCoordinateModel(obj.Id))
            .ToList();

    /// <summary>
    /// Creates a ParameterValueModel with test data.
    /// </summary>
    private ParameterValueModel CreateParameterValueModel()
    {
        var parameterModel = fixture.Build<ParameterStateModel>()
            .With(p => p.Id, fixture.Create<long>())
            .With(p => p.Name, fixture.Create<string>()[..8])
            .Create();

        return fixture.Build<ParameterValueModel>()
            .With(pv => pv.Id, fixture.Create<long>())
            .With(pv => pv.Value, fixture.Create<string>()[..10])
            .With(pv => pv.ParameterId, parameterModel.Id)
            .With(pv => pv.Parameter, parameterModel)
            .Create();
    }

    /// <summary>
    /// Creates ParameterValueModel list.
    /// </summary>
    private List<ParameterValueModel> CreateParameterValueModelList(int count) =>
        Enumerable.Range(0, count)
            .Select(_ => CreateParameterValueModel())
            .ToList();
}
