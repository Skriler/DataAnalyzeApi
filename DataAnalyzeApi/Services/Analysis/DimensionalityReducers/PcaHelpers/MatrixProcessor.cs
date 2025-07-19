namespace DataAnalyzeApi.Services.Analysis.DimensionalityReducers.PcaHelpers;

/// <summary>
/// Helper class for matrix operations required in PCA.
/// </summary>
public class MatrixProcessor
{
    /// <summary>
    /// Centers the matrix by subtracting column means from each element.
    /// </summary>
    public void CenterMatrix(double[,] matrix)
    {
        int rowCount = matrix.GetLength(0);
        int columnCount = matrix.GetLength(1);
        var columnMeans = new double[columnCount];

        // Calculate column means
        for (int columnIndex = 0; columnIndex < columnCount; ++columnIndex)
        {
            double columnSum = 0;

            for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
            {
                columnSum += matrix[rowIndex, columnIndex];
            }

            columnMeans[columnIndex] = columnSum / rowCount;
        }

        // Subtract means from matrix elements
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            for (int columnIndex = 0; columnIndex < columnCount; ++columnIndex)
            {
                matrix[rowIndex, columnIndex] -= columnMeans[columnIndex];
            }
        }
    }

    /// <summary>
    /// Computes covariance matrix from centered data matrix.
    /// </summary>
    public double[,] ComputeCovarianceMatrix(double[,] centeredMatrix)
    {
        int rowCount = centeredMatrix.GetLength(0);
        int columnCount = centeredMatrix.GetLength(1);
        var covarianceMatrix = new double[columnCount, columnCount];

        for (int firstColumnIndex = 0; firstColumnIndex < columnCount; ++firstColumnIndex)
        {
            for (int secondColumnIndex = firstColumnIndex; secondColumnIndex < columnCount; ++secondColumnIndex)
            {
                double covarianceSum = 0;

                for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
                {
                    covarianceSum += centeredMatrix[rowIndex, firstColumnIndex] *
                        centeredMatrix[rowIndex, secondColumnIndex];
                }

                double covarianceValue = covarianceSum / (rowCount - 1);

                covarianceMatrix[firstColumnIndex, secondColumnIndex] = covarianceValue;
                covarianceMatrix[secondColumnIndex, firstColumnIndex] = covarianceValue;
            }
        }

        return covarianceMatrix;
    }
}
