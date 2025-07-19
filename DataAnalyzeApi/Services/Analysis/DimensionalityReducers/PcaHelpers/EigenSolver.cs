using DataAnalyzeApi.Models.Domain.DimensionalityReduction.Pca;

namespace DataAnalyzeApi.Services.Analysis.DimensionalityReducers.PcaHelpers;

/// <summary>
/// Helper class for computing eigenvalues and eigenvectors using Jacobi method.
/// </summary>
public class EigenSolver
{
    private const int MaxIterations = 1000;
    private const double Tolerance = 1e-10;

    /// <summary>
    /// Computes eigenvalues and eigenvectors of a symmetric matrix using Jacobi method.
    /// </summary>
    public EigenDecomposition ComputeEigenVectorsAndValues(double[,] matrix)
    {
        int matrixSize = matrix.GetLength(0);
        var eigenValues = new double[matrixSize];

        var eigenVectors = CreateIdentityMatrix(matrixSize);
        var workingMatrix = CopyMatrix(matrix);

        for (int i = 0; i < MaxIterations; ++i)
        {
            var pivotElement = FindLargestOffDiagonalElement(workingMatrix);

            if (pivotElement.Value < Tolerance)
                break;

            double rotationAngle = CalculateRotationAngle(
                workingMatrix,
                pivotElement.Row,
                pivotElement.Column);

            double cosineValue = Math.Cos(rotationAngle);
            double sineValue = Math.Sin(rotationAngle);

            ApplyJacobiRotation(
                workingMatrix,
                eigenVectors,
                pivotElement.Row,
                pivotElement.Column,
                cosineValue,
                sineValue);
        }

        ExtractEigenValues(workingMatrix, eigenValues);

        return new EigenDecomposition(eigenVectors, eigenValues);
    }

    /// <summary>
    /// Creates an identity matrix of given size.
    /// </summary>
    private static double[,] CreateIdentityMatrix(int size)
    {
        var identityMatrix = new double[size, size];

        for (int diagonalIndex = 0; diagonalIndex < size; ++diagonalIndex)
        {
            identityMatrix[diagonalIndex, diagonalIndex] = 1.0;
        }

        return identityMatrix;
    }

    /// <summary>
    /// Creates a copy of the input matrix.
    /// </summary>
    private static double[,] CopyMatrix(double[,] matrix)
    {
        int size = matrix.GetLength(0);
        var copy = new double[size, size];

        for (int rowIndex = 0; rowIndex < size; ++rowIndex)
        {
            for (int columnIndex = 0; columnIndex < size; ++columnIndex)
            {
                copy[rowIndex, columnIndex] = matrix[rowIndex, columnIndex];
            }
        }

        return copy;
    }

    /// <summary>
    /// Finds the largest off-diagonal element in the matrix.
    /// </summary>
    private static PivotElement FindLargestOffDiagonalElement(double[,] matrix)
    {
        int matrixSize = matrix.GetLength(0);
        double maxValue = 0;
        int pivotRow = 0;
        int pivotColumn = 1;

        for (int rowIndex = 0; rowIndex < matrixSize; ++rowIndex)
        {
            for (int columnIndex = rowIndex + 1; columnIndex < matrixSize; ++columnIndex)
            {
                double absoluteValue = Math.Abs(matrix[rowIndex, columnIndex]);

                if (absoluteValue <= maxValue)
                    continue;

                maxValue = absoluteValue;
                pivotRow = rowIndex;
                pivotColumn = columnIndex;
            }
        }

        return new PivotElement(pivotRow, pivotColumn, maxValue);
    }

    /// <summary>
    /// Calculates the rotation angle for Jacobi rotation.
    /// </summary>
    private static double CalculateRotationAngle(
        double[,] matrix,
        int pivotRow,
        int pivotColumn)
    {
        double diagonalDifference =
            matrix[pivotRow, pivotRow] -
            matrix[pivotColumn, pivotColumn];

        if (Math.Abs(diagonalDifference) < Tolerance)
        {
            return Math.PI / 4;
        }

        double numerator = 2 * matrix[pivotRow, pivotColumn];
        double denominator = diagonalDifference;

        return 0.5 * Math.Atan(numerator / denominator);
    }

    /// <summary>
    /// Applies Jacobi rotation to eliminate off-diagonal element.
    /// </summary>
    private static void ApplyJacobiRotation(
        double[,] matrix,
        double[,] eigenVectors,
        int pivotRow,
        int pivotColumn,
        double cosineValue,
        double sineValue)
    {
        UpdateOffDiagonalElements(matrix, pivotRow, pivotColumn, cosineValue, sineValue);
        UpdateDiagonalElements(matrix, pivotRow, pivotColumn, cosineValue, sineValue);
        UpdateEigenVectors(eigenVectors, pivotRow, pivotColumn, cosineValue, sineValue);
    }

    /// <summary>
    /// Updates off-diagonal matrix elements during Jacobi rotation.
    /// </summary>
    private static void UpdateOffDiagonalElements(
        double[,] matrix,
        int pivotRow,
        int pivotColumn,
        double cosineValue,
        double sineValue)
    {
        int matrixSize = matrix.GetLength(0);

        for (int elementIndex = 0; elementIndex < matrixSize; ++elementIndex)
        {
            if (elementIndex == pivotRow || elementIndex == pivotColumn)
                continue;

            double matrixElementPivotRow = matrix[elementIndex, pivotRow];
            double matrixElementPivotColumn = matrix[elementIndex, pivotColumn];

            matrix[elementIndex, pivotRow] =
                (cosineValue * matrixElementPivotRow) -
                (sineValue * matrixElementPivotColumn);

            matrix[pivotRow, elementIndex] = matrix[elementIndex, pivotRow];

            matrix[elementIndex, pivotColumn] =
                (sineValue * matrixElementPivotRow) +
                (cosineValue * matrixElementPivotColumn);

            matrix[pivotColumn, elementIndex] = matrix[elementIndex, pivotColumn];
        }
    }

    /// <summary>
    /// Updates diagonal matrix elements during Jacobi rotation.
    /// </summary>
    private static void UpdateDiagonalElements(
        double[,] matrix,
        int pivotRow,
        int pivotColumn,
        double cosineValue,
        double sineValue)
    {
        double originalPivotRowDiagonal = matrix[pivotRow, pivotRow];
        double originalPivotColumnDiagonal = matrix[pivotColumn, pivotColumn];
        double originalOffDiagonal = matrix[pivotRow, pivotColumn];

        matrix[pivotRow, pivotRow] = (cosineValue * cosineValue * originalPivotRowDiagonal)
                                   - (2 * cosineValue * sineValue * originalOffDiagonal)
                                   + (sineValue * sineValue * originalPivotColumnDiagonal);

        matrix[pivotColumn, pivotColumn] = (sineValue * sineValue * originalPivotRowDiagonal)
                                         + (2 * cosineValue * sineValue * originalOffDiagonal)
                                         + (cosineValue * cosineValue * originalPivotColumnDiagonal);

        matrix[pivotRow, pivotColumn] = 0;
        matrix[pivotColumn, pivotRow] = 0;
    }

    /// <summary>
    /// Updates eigenvector matrix during Jacobi rotation.
    /// </summary>
    private static void UpdateEigenVectors(
        double[,] eigenVectors,
        int pivotRow,
        int pivotColumn,
        double cosineValue,
        double sineValue)
    {
        int matrixSize = eigenVectors.GetLength(0);

        for (int vectorIndex = 0; vectorIndex < matrixSize; ++vectorIndex)
        {
            double vectorElementPivotRow = eigenVectors[vectorIndex, pivotRow];
            double vectorElementPivotColumn = eigenVectors[vectorIndex, pivotColumn];

            eigenVectors[vectorIndex, pivotRow] =
                (cosineValue * vectorElementPivotRow) -
                (sineValue * vectorElementPivotColumn);

            eigenVectors[vectorIndex, pivotColumn] =
                (sineValue * vectorElementPivotRow) +
                (cosineValue * vectorElementPivotColumn);
        }
    }

    /// <summary>
    /// Extracts eigenvalues from diagonal elements of the matrix.
    /// </summary>
    private static void ExtractEigenValues(
        double[,] matrix,
        double[] eigenValues)
    {
        for (int diagonalIndex = 0; diagonalIndex < eigenValues.Length; ++diagonalIndex)
        {
            eigenValues[diagonalIndex] = matrix[diagonalIndex, diagonalIndex];
        }
    }
}
