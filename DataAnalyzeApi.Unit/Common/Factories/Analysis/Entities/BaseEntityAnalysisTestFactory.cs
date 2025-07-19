using AutoFixture;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.Entities;

namespace DataAnalyzeApi.Unit.Common.Factories.Analysis.Entities;

public abstract class BaseEntityAnalysisTestFactory
{
    protected readonly Fixture fixture = new();

    /// <summary>
    /// Creates a DataObject entity with test data.
    /// </summary>
    protected DataObject CreateDataObject()
    {
        return fixture.Build<DataObject>()
            .With(d => d.Id, fixture.Create<short>())
            .With(d => d.Name, fixture.Create<string>()[..10])
            .Without(d => d.DatasetId)
            .Without(d => d.Dataset)
            .Without(d => d.Values)
            .Create();
    }

    /// <summary>
    /// Creates a list of DataObject entities.
    /// </summary>
    protected List<DataObject> CreateDataObjectList(int count) =>
        Enumerable.Range(0, count)
            .Select(_ => CreateDataObject())
            .ToList();

    /// <summary>
    /// Creates a DataObjectAnalysisDto with test data.
    /// </summary>
    protected DataObjectAnalysisDto CreateDataObjectAnalysisDto()
    {
        return fixture.Build<DataObjectAnalysisDto>()
            .With(d => d.Id, fixture.Create<short>())
            .With(d => d.Name, fixture.Create<string>()[..10])
            .Without(d => d.ParameterValues)
            .Create();
    }

    /// <summary>
    /// Creates a list of DataObjectAnalysisDto.
    /// </summary>
    protected List<DataObjectAnalysisDto> CreateDataObjectAnalysisDtoList(int count) =>
        Enumerable.Range(0, count)
            .Select(_ => CreateDataObjectAnalysisDto())
            .ToList();
}
