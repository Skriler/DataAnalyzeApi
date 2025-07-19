using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Services.Analysis.DimensionalityReducers;
using DataAnalyzeApi.Services.Analysis.DimensionalityReducers.PcaHelpers;
using DataAnalyzeApi.Unit.Common.Assertions.Analysis;
using DataAnalyzeApi.Unit.Common.Factories.Datasets.Models;
using DataAnalyzeApi.Unit.Common.Models.Analysis;

namespace DataAnalyzeApi.Unit.Tests.Services.Analysis.DimensionalityReducers;

[Trait("Category", "Unit")]
[Trait("Component", "Analysis")]
[Trait("SubComponent", "DimensionalityReducers")]
public class PcaReducerTests
{
    private readonly DataObjectModelFactory dataObjectModelFactory;
    private readonly PcaReducer pcaReducer;

    public PcaReducerTests()
    {
        dataObjectModelFactory = new DataObjectModelFactory();

        var matrixProcessor = new MatrixProcessor();
        var eigenSolver = new EigenSolver();
        pcaReducer = new PcaReducer(matrixProcessor, eigenSolver);
    }

    /// <summary>
    /// Verifies that PCA returns empty result when input is null
    /// </summary>
    [Fact]
    public void ReduceDimensions_WhenObjectsIsNull_ReturnsEmptyResult()
    {
        // Act
        var result = pcaReducer.ReduceDimensions(null!);

        // Assert
        Assert.Empty(result.DataObjectCoordinates);
    }

    /// <summary>
    /// Verifies that PCA returns empty result when input collection is empty
    /// </summary>
    [Fact]
    public void ReduceDimensions_WhenObjectsIsEmpty_ReturnsEmptyResult()
    {
        // Arrange
        var emptyObjects = new List<DataObjectModel>();

        // Act
        var result = pcaReducer.ReduceDimensions(emptyObjects);

        // Assert
        Assert.Empty(result.DataObjectCoordinates);
    }

    /// <summary>
    /// Verifies that PCA handles objects with no features by returning zero coordinates
    /// </summary>
    [Fact]
    public void ReduceDimensions_WhenNoFeatures_ReturnsZeroCoordinates()
    {
        // Arrange
        var normalizedObjects = new List<NormalizedDataObject> { new(), new(), };
        var objects = dataObjectModelFactory.CreateNormalizedList(normalizedObjects);

        // Act
        var result = pcaReducer.ReduceDimensions(objects);

        // Assert
        Assert.Equal(2, result.DataObjectCoordinates.Count);
        DimensionalityReductionAssertions.AssertCoordinatesAreAtOrigin(result.DataObjectCoordinates);
    }

    /// <summary>
    /// Verifies that PCA correctly processes normalized numeric features and produces valid coordinates
    /// </summary>
    [Fact]
    public void ReduceDimensions_WithOnlyNumericFeatures_ProducesCorrectCoordinates()
    {
        // Arrange
        var normalizedObjects = new List<NormalizedDataObject>
        {
            new() { NumericValues = [0.25, 0.2] },
            new() { NumericValues = [0.5, 0.4] },
            new() { NumericValues = [0.75, 0.6] },
            new() { NumericValues = [1.0, 0.8] },
        };

        var objects = dataObjectModelFactory.CreateNormalizedList(normalizedObjects);

        // Act
        var result = pcaReducer.ReduceDimensions(objects);

        // Assert
        Assert.Equal(4, result.DataObjectCoordinates.Count);
        DimensionalityReductionAssertions.AssertObjectIdsArePreserved(result.DataObjectCoordinates);
        DimensionalityReductionAssertions.AssertCoordinatesAreFinite(result.DataObjectCoordinates);
        DimensionalityReductionAssertions.AssertNotAllCoordinatesAreAtOrigin(result.DataObjectCoordinates);
        DimensionalityReductionAssertions.AssertFirstPrincipalComponentHasVariance(result.DataObjectCoordinates);
    }

    /// <summary>
    /// Verifies that PCA handles categorical features correctly and produces valid coordinates
    /// </summary>
    [Fact]
    public void ReduceDimensions_WithOnlyCategoricalFeatures_ProducesValidCoordinates()
    {
        // Arrange
        var normalizedObjects = new List<NormalizedDataObject>
        {
            new() { CategoricalValues = [[1, 0, 0], [0, 1]] },
            new() { CategoricalValues = [[0, 1, 0], [0, 1]] },
            new() { CategoricalValues = [[0, 0, 1], [1, 0]] },
            new() { CategoricalValues = [[1, 0, 0], [1, 0]] },
        };

        var objects = dataObjectModelFactory.CreateNormalizedList(normalizedObjects);

        // Act
        var result = pcaReducer.ReduceDimensions(objects);

        // Assert
        Assert.Equal(4, result.DataObjectCoordinates.Count);
        DimensionalityReductionAssertions.AssertObjectIdsArePreserved(result.DataObjectCoordinates);
        DimensionalityReductionAssertions.AssertCoordinatesAreFinite(result.DataObjectCoordinates);
        DimensionalityReductionAssertions.AssertCoordinatesHaveVariance(result.DataObjectCoordinates);
    }

    /// <summary>
    /// Verifies that PCA handles mixed numeric and categorical features correctly
    /// </summary>
    [Fact]
    public void ReduceDimensions_WithMixedFeatures_ProducesValidCoordinates()
    {
        // Arrange
        var normalizedObjects = new List<NormalizedDataObject>
        {
            new()
            {
                NumericValues = [0.25, 0.4],
                CategoricalValues = [[1, 0], [0, 1]],
            },
            new()
            {
                NumericValues = [0.5, 0.6],
                CategoricalValues = [[0, 1], [0, 1]],
            },
            new()
            {
                NumericValues = [0.75, 0.8],
                CategoricalValues = [[1, 0], [1, 0]],
            },
            new()
            {
                NumericValues = [1.0, 1.0],
                CategoricalValues = [[0, 1], [1, 0]],
            }
        };

        var objects = dataObjectModelFactory.CreateNormalizedList(normalizedObjects);

        // Act
        var result = pcaReducer.ReduceDimensions(objects);

        // Assert
        Assert.Equal(4, result.DataObjectCoordinates.Count);
        DimensionalityReductionAssertions.AssertObjectIdsArePreserved(result.DataObjectCoordinates);
        DimensionalityReductionAssertions.AssertCoordinatesAreFinite(result.DataObjectCoordinates);
    }

    /// <summary>
    /// Verifies that PCA handles identical data points gracefully without errors
    /// </summary>
    [Fact]
    public void ReduceDimensions_WithIdenticalPoints_HandlesGracefully()
    {
        // Arrange
        var normalizedObjects = new List<NormalizedDataObject>
        {
            new() { NumericValues = [0.25, 0.4] },
            new() { NumericValues = [0.25, 0.4] },
            new() { NumericValues = [0.25, 0.4] },
        };

        var objects = dataObjectModelFactory.CreateNormalizedList(normalizedObjects);

        // Act
        var result = pcaReducer.ReduceDimensions(objects);

        // Assert
        Assert.Equal(3, result.DataObjectCoordinates.Count);
        DimensionalityReductionAssertions.AssertObjectIdsArePreserved(result.DataObjectCoordinates);
        DimensionalityReductionAssertions.AssertCoordinatesAreAtOrigin(result.DataObjectCoordinates);
    }

    /// <summary>
    /// Verifies that PCA correctly reduces high-dimensional data to 2D coordinates
    /// </summary>
    [Fact]
    public void ReduceDimensions_WithHighDimensionalData_ReducesToTwoDimensions()
    {
        // Arrange
        var normalizedObjects = new List<NormalizedDataObject>
        {
            new() { NumericValues = [0.2, 0.2, 0.3, 0.4, 0.5] },
            new() { NumericValues = [0.4, 0.4, 0.6, 0.8, 1.0] },
            new() { NumericValues = [0.3, 0.3, 0.45, 0.6, 0.75] },
            new() { NumericValues = [0.1, 0.1, 0.15, 0.2, 0.25] },
        };

        var objects = dataObjectModelFactory.CreateNormalizedList(normalizedObjects);

        // Act
        var result = pcaReducer.ReduceDimensions(objects);

        // Assert
        Assert.Equal(4, result.DataObjectCoordinates.Count);
        DimensionalityReductionAssertions.AssertObjectIdsArePreserved(result.DataObjectCoordinates);
        DimensionalityReductionAssertions.AssertCoordinatesAreFinite(result.DataObjectCoordinates);
        DimensionalityReductionAssertions.AssertFirstPrincipalComponentHasVariance(result.DataObjectCoordinates);
    }
}
