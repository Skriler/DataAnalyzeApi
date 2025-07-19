using DataAnalyzeApi.Exceptions.Vector;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Enum;
using DataAnalyzeApi.Services.Analysis.Comparers;
using DataAnalyzeApi.Unit.Common.Factories.Datasets.Models;
using DataAnalyzeApi.Unit.Common.Models.Analysis;
using DataAnalyzeApi.Unit.Common.TestData.Comparers;
using Moq;

namespace DataAnalyzeApi.Unit.Tests.Services.Analysis.Comparers;

[Trait("Category", "Unit")]
[Trait("Component", "Analysis")]
[Trait("SubComponent", "Comparer")]
public class SimilarityComparerTests
{
    private readonly DatasetModelFactory datasetModelFactory;
    private readonly Mock<ICompare> comparerMock;
    private readonly SimilarityComparer similarityComparer;

    public SimilarityComparerTests()
    {
        datasetModelFactory = new();
        comparerMock = new Mock<ICompare>();
        similarityComparer = new SimilarityComparer(comparerMock.Object);
    }

    [Fact]
    public void CompareAllObjects_ShouldThrowException_WhenDatasetIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => similarityComparer.CompareAllObjects(null!));
    }

    [Fact]
    public void CompareAllObjects_ShouldThrowException_WhenDatasetHasNoObjects()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>();
        var datasetModel = datasetModelFactory.Create(rawObjects);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => similarityComparer.CompareAllObjects(datasetModel));
    }

    [Fact]
    public void CompareAllObjects_ShouldThrowException_WhenDatasetHasNoParameters()
    {
        // Arrange
        var parameterStates = new List<ParameterStateModel>();
        var rawObjects = new List<RawDataObject>
        {
            new()
            {
                Values =  ["0.5", "0.2"]
            },
        };

        var dataObjects = datasetModelFactory.Create(rawObjects);
        var datasetModel = new DatasetModel(0, string.Empty, parameterStates, dataObjects.Objects);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => similarityComparer.CompareAllObjects(datasetModel));
    }

   [Fact]
    public void CompareAllObjects_ShouldThrowException_WhenVectorIsNull()
    {
        // Arrange
        var parameterStates = new List<ParameterStateModel>
        {
            new(0, string.Empty, ParameterType.Categorical, true, 1),
        };
        var dataObjects = new List<DataObjectModel>()
        {
            new(0, string.Empty, null!),
            new(0, string.Empty, null!),
        };

        var datasetModel = new DatasetModel(0, string.Empty, parameterStates, dataObjects);

        // Act & Assert
        Assert.Throws<VectorNullException>(() => similarityComparer.CompareAllObjects(datasetModel));
    }

    [Fact]
    public void CompareAllObjects_ShouldThrowException_WhenVectorsAreEmpty()
    {
        // Arrange
        var parameterStates = new List<ParameterStateModel>
        {
            new(0, string.Empty, ParameterType.Categorical, true, 1),
        };
        var dataObjects = new List<DataObjectModel>()
        {
            new(0, string.Empty, []),
            new(0, string.Empty, []),
        };

        var datasetModel = new DatasetModel(0, string.Empty, parameterStates, dataObjects);

        // Act & Assert
        Assert.Throws<EmptyVectorException>(() => similarityComparer.CompareAllObjects(datasetModel));
    }

    [Fact]
    public void CompareAllObjects_ShouldThrowException_WhenVectorsHaveDifferentLengths()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>
        {
            new()
            {
                Values = ["0.2", "0.4"],
            },

            new()
            {
                Values = ["0.6", "0.8", "0.3"],
            },
        };

        var datasetModel = datasetModelFactory.Create(rawObjects);

        // Act & Assert
        Assert.Throws<VectorLengthMismatchException>(() => similarityComparer.CompareAllObjects(datasetModel));
    }

    [Theory]
    [MemberData(
        nameof(SimilarityComparerTestData.CompareAllObjectsTestCases),
        MemberType = typeof(SimilarityComparerTestData))]
    public void Calculate_ReturnsExpectedDistance(SimilarityComparerTestCase testCase)
    {
        // Arrange
        var datasetModel = datasetModelFactory.Create(testCase.Objects);
        SetupDistanceCalculatorMock(testCase.PairwiseSimilarities);

        // Act
        var similarities = similarityComparer.CompareAllObjects(datasetModel);

        // Assert
        foreach (var pairSimilarities in testCase.ExpectedSimilarities)
        {
            var similarity = similarities.First(p => p.ObjectA.Id == pairSimilarities.ObjectAIndex
                                                  && p.ObjectB.Id == pairSimilarities.ObjectBIndex);

            Assert.NotNull(similarity);
            Assert.Equal(
                pairSimilarities.SimilarityPercentage,
                similarity.SimilarityPercentage,
                precision: 2);
        }
    }

    /// <summary>
    /// Sets up the mock ICompare to return predefined similarity percentages between object pairs.
    /// </summary>
    protected void SetupDistanceCalculatorMock(List<ParameterValuePairSimilarity> pairwiseSimilarities)
    {
        var similaritiesLookup = new Dictionary<(string, string), double>();

        foreach (var pair in pairwiseSimilarities)
        {
            similaritiesLookup[(pair.ValueA, pair.ValueB)] = pair.SimilarityPercentage;
        }

        comparerMock
            .Setup(c => c.Compare(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<double>()))
            .Returns((
                string valA,
                string valB,
                double _) =>
            {
                if (valA == valB)
                {
                    return 1;
                }

                if (similaritiesLookup.TryGetValue((valA, valB), out double similarityPercentage))
                {
                    return similarityPercentage;
                }

                // Throw error if there is no pair similarity percentage in the test case.
                throw new InvalidOperationException($"No mock distance defined for values {valA} and {valB}.");
            });
    }
}
