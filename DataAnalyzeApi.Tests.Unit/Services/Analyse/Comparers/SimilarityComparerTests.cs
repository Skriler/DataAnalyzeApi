using DataAnalyzeApi.Exceptions.Vector;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Services.Analyse.Comparers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories.Models;
using Moq;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Comparers;

public class SimilarityComparerTests
{
    protected readonly DatasetModelFactory datasetModelFactory;
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
    public void CCompareAllObjects_ShouldThrowException_WhenDatasetHasNoObjects()
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
        var rawValue = new RawDataObject() { Values =  { "0.5", "0.2" } };
        var parameterStates = new List<ParameterStateModel>();

        var dataObjects = new List<DataObjectModel>()
        {
            datasetModelFactory.CreateDataObjectModel(rawValue),
        };

        var datasetModel = new DatasetModel(0, string.Empty, parameterStates, dataObjects);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => similarityComparer.CompareAllObjects(datasetModel));
    }

    [Fact]
    public void CompareAllObjects_ShouldThrowException_WhenVectorIsNull()
    {
        // Arrange
        var rawValue = new RawDataObject() { Values = { "0.5", "0.2" } };
        var parameterStates = new List<ParameterStateModel>();

        var dataObjects = new List<DataObjectModel>()
        {
            datasetModelFactory.CreateDataObjectModel(rawValue),
        };

        var datasetModel = new DatasetModel(0, string.Empty, parameterStates, dataObjects);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => similarityComparer.CompareAllObjects(datasetModel));
    }

    [Fact]
    public void CompareAllObjects_ShouldThrowException_WhenVectorsAreEmpty()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>()
        {
            new RawDataObject
            {
                Values = new List<string>(),
            },
            new RawDataObject
            {
                Values = new List<string>(),
            },
        };

        var datasetModel = datasetModelFactory.Create(rawObjects);

        // Act & Assert
        Assert.Throws<EmptyVectorException>(() => similarityComparer.CompareAllObjects(datasetModel));
    }

    [Fact]
    public void CompareAllObjects_ShouldThrowException_WhenVectorsHaveDifferentLengths()
    {
        // Arrange
        var rawObjects = new List<RawDataObject>()
        {
            new RawDataObject
            {
                Values = { "0.2", "0.4" },
            },

            new RawDataObject
            {
                Values = { "0.6", "0.8", "0.3" },
            },
        };

        var datasetModel = datasetModelFactory.Create(rawObjects);

        // Act & Assert
        Assert.Throws<VectorLengthMismatchException>(() => similarityComparer.CompareAllObjects(datasetModel));
    }

    [Theory]
    [InlineData(new string[] { "0.2", "0.3", "0.4" }, new string[] { "0.7", "0.3", "0.5" }, 1)]
    public void Calculate_ReturnsExpectedDistance(string[] valuesA, string[] valuesB, double expectedDistance)
    {
        // Arrange
        var rawObjects = new List<RawDataObject>()
    {
        new RawDataObject
        {
            Values = valuesA.ToList(),
        },

        new RawDataObject
        {
            Values = valuesB.ToList(),
        },
    };

        var datasetModel = datasetModelFactory.Create(rawObjects);

        // Act
        var similarityPair = similarityComparer.CompareAllObjects(datasetModel);

        // Assert
        //Assert.Equal(expectedDistance, distance, precision: 4);
        //Assert.Equal(expectedAverageDistance, result, precision: 4);
    }
}
