using AutoFixture;
using DataAnalyzeApi.Models.Domain.Clustering.KMeans;
using DataAnalyzeApi.Tests.Common.Factories.Models;
using DataAnalyzeApi.Tests.Common.Models.Analyse;

namespace DataAnalyzeApi.Tests.Common.Factories;

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
