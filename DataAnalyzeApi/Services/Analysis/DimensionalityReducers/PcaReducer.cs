using DataAnalyzeApi.Extensions.Model;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;
using DataAnalyzeApi.Models.Domain.DimensionalityReduction;
using DataAnalyzeApi.Models.Domain.DimensionalityReduction.Pca;
using DataAnalyzeApi.Services.Analysis.DimensionalityReducers.PcaHelpers;

namespace DataAnalyzeApi.Services.Analysis.DimensionalityReducers;

public class PcaReducer(MatrixProcessor matrixProcessor, EigenSolver eigenSolver) : IDimensionalityReducer
{
    private const int TargetDimensions = 2;

    private readonly MatrixProcessor matrixProcessor = matrixProcessor;
    private readonly EigenSolver eigenSolver = eigenSolver;

    /// <summary>
    /// Reduces dimensionality of data objects using Principal Component Analysis.
    /// </summary>
    public DimensionalityReductionResult ReduceDimensions(List<DataObjectModel> objects)
    {
        var coordinates = new List<DataObjectCoordinateModel>();

        if (objects == null || objects.Count == 0)
            return new DimensionalityReductionResult(coordinates);

        var featureMatrix = BuildFeatureMatrix(objects);

        if (featureMatrix.GetLength(1) == 0)
        {
            coordinates = objects
                .Select(dataObject => new DataObjectCoordinateModel(dataObject.Id, 0, 0))
                .ToList();

            return new DimensionalityReductionResult(coordinates);
        }

        matrixProcessor.CenterMatrix(featureMatrix);

        var covarianceMatrix = matrixProcessor.ComputeCovarianceMatrix(featureMatrix);
        var eigenDecomposition = eigenSolver.ComputeEigenVectorsAndValues(covarianceMatrix);

        var projectionMatrix = CreateProjectionMatrix(eigenDecomposition);
        var projectedData = ProjectData(featureMatrix, projectionMatrix);

        coordinates = CreateCoordinates(objects, projectedData);

        return new DimensionalityReductionResult(coordinates);
    }

    /// <summary>
    /// Builds feature matrix from data objects combining numeric and categorical features.
    /// </summary>
    private static double[,] BuildFeatureMatrix(List<DataObjectModel> objects)
    {
        if (objects.Count == 0)
            return new double[0, 0];

        var firstObject = objects[0];
        var numericParams = firstObject.Values.OfParameterTypeOrdered<NormalizedNumericValueModel>();
        var categoricalParams = firstObject.Values.OfParameterTypeOrdered<NormalizedCategoricalValueModel>();

        int numericFeatureCount = numericParams.Count;
        int categoricalFeatureCount = categoricalParams.Sum(param => param.OneHotValues.Length);
        int totalFeatureCount = numericFeatureCount + categoricalFeatureCount;

        if (totalFeatureCount == 0)
            return new double[objects.Count, 0];

        var matrix = new double[objects.Count, totalFeatureCount];

        for (int objectIndex = 0; objectIndex < objects.Count; ++objectIndex)
        {
            FillMatrixRow(matrix, objects[objectIndex], objectIndex, numericFeatureCount);
        }

        return matrix;
    }

    /// <summary>
    /// Fills a single row of the feature matrix with object's numeric and categorical values.
    /// </summary>
    private static void FillMatrixRow(
        double[,] matrix,
        DataObjectModel dataObject,
        int rowIndex,
        int numericFeatureCount)
    {
        var numericParams = dataObject.Values.OfParameterTypeOrdered<NormalizedNumericValueModel>();
        var categoricalParams = dataObject.Values.OfParameterTypeOrdered<NormalizedCategoricalValueModel>();

        // Fill numeric features
        for (int numericIndex = 0; numericIndex < numericParams.Count; ++numericIndex)
        {
            matrix[rowIndex, numericIndex] = numericParams[numericIndex].NormalizedValue;
        }

        // Fill categorical features (one-hot encoded)
        int currentOffset = numericFeatureCount;

        for (int categoricalIndex = 0; categoricalIndex < categoricalParams.Count; ++categoricalIndex)
        {
            var oneHotValues = categoricalParams[categoricalIndex].OneHotValues;

            for (int oneHotIndex = 0; oneHotIndex < oneHotValues.Length; ++oneHotIndex)
            {
                matrix[rowIndex, currentOffset + oneHotIndex] = oneHotValues[oneHotIndex];
            }

            currentOffset += oneHotValues.Length;
        }
    }

    /// <summary>
    /// Creates projection matrix from eigenvectors using the top 2 components.
    /// </summary>
    private static double[,] CreateProjectionMatrix(EigenDecomposition eigenDecomposition)
    {
        var sortedIndices = eigenDecomposition.Values
            .Select((value, index) => new { Value = value, Index = index })
            .OrderByDescending(eigenPair => eigenPair.Value)
            .Take(TargetDimensions)
            .Select(eigenPair => eigenPair.Index)
            .ToArray();

        var projectionMatrix = new double[eigenDecomposition.Vectors.GetLength(0), TargetDimensions];

        for (int featureIndex = 0; featureIndex < eigenDecomposition.Vectors.GetLength(0); ++featureIndex)
        {
            projectionMatrix[featureIndex, 0] = sortedIndices.Length > 0
                ? eigenDecomposition.Vectors[featureIndex, sortedIndices[0]]
                : 0;

            projectionMatrix[featureIndex, 1] = sortedIndices.Length > 1
                ? eigenDecomposition.Vectors[featureIndex, sortedIndices[1]]
                : 0;
        }

        return projectionMatrix;
    }

    /// <summary>
    /// Projects data onto the lower-dimensional space using the projection matrix.
    /// </summary>
    private static double[,] ProjectData(
        double[,] data,
        double[,] projectionMatrix)
    {
        int dataPointCount = data.GetLength(0);
        int projectedDimensionCount = projectionMatrix.GetLength(1);
        var projectedData = new double[dataPointCount, projectedDimensionCount];

        for (int dataPointIndex = 0; dataPointIndex < dataPointCount; ++dataPointIndex)
        {
            for (int dimensionIndex = 0; dimensionIndex < projectedDimensionCount; ++dimensionIndex)
            {
                double projectedValue = 0;

                for (int featureIndex = 0; featureIndex < data.GetLength(1); ++featureIndex)
                {
                    projectedValue +=
                        data[dataPointIndex, featureIndex] *
                        projectionMatrix[featureIndex, dimensionIndex];
                }

                projectedData[dataPointIndex, dimensionIndex] = projectedValue;
            }
        }

        return projectedData;
    }

    /// <summary>
    /// Creates coordinate models from projected data and original objects.
    /// </summary>
    private static List<DataObjectCoordinateModel> CreateCoordinates(
        List<DataObjectModel> objects,
        double[,] projectedData)
    {
        var coordinates = new List<DataObjectCoordinateModel>();

        for (int objectIndex = 0; objectIndex < objects.Count; ++objectIndex)
        {
            coordinates.Add(new DataObjectCoordinateModel(
                objects[objectIndex].Id,
                projectedData[objectIndex, 0],
                projectedData[objectIndex, 1]
            ));
        }

        return coordinates;
    }
}
