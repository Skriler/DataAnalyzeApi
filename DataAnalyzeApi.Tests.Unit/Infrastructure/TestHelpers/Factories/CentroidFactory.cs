using AutoFixture;
using DataAnalyzeApi.Models.Domain.Clustering.KMeans;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories.Models;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories;

public class CentroidFactory
{
    private readonly Fixture fixture;
    private readonly ValueModelFactory valueModelFactory;

    public CentroidFactory()
    {
        fixture = new Fixture();
        valueModelFactory = new ValueModelFactory(fixture);
    }

    /// <summary>
    /// Creates a Centroid with numeric and categorical values.
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
