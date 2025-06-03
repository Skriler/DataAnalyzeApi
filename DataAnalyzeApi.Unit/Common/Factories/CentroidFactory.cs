using AutoFixture;
using DataAnalyzeApi.Models.Domain.Clustering.KMeans;
using DataAnalyzeApi.Unit.Common.Factories.Models;
using DataAnalyzeApi.Unit.Common.Models.Analysis;

namespace DataAnalyzeApi.Unit.Common.Factories;

public class CentroidFactory
{
    private readonly Fixture fixture;
    private readonly ParameterValueModelFactory valueModelFactory;

    public CentroidFactory()
    {
        fixture = new Fixture();
        valueModelFactory = new ParameterValueModelFactory(fixture);
    }

    /// <summary>
    /// Creates a Centroid with NormalizedDataObject data.
    /// </summary>
    public Centroid Create(NormalizedDataObject centroid)
    {
        var valueModels = valueModelFactory.CreateNormalizedList(
            centroid.NumericValues,
            centroid.CategoricalValues);

        return fixture.Build<Centroid>()
            .With(obj => obj.Values, valueModels)
            .Create();
    }
}
