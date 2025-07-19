using DataAnalyzeApi.Models.Domain.DimensionalityReduction;

namespace DataAnalyzeApi.Unit.Common.Assertions.Analysis;

public static class DimensionalityReductionAssertions
{
    /// <summary>
    /// Verifies that all coordinates are finite numbers (not NaN or infinite)
    /// </summary>
    public static void AssertCoordinatesAreFinite(List<DataObjectCoordinateModel> coordinates)
    {
        Assert.All(coordinates, coord =>
        {
            Assert.True(
                double.IsFinite(coord.X),
                $"X coordinate {coord.X} is not finite"
            );

            Assert.True(
                double.IsFinite(coord.Y),
                $"Y coordinate {coord.Y} is not finite"
            );
        });
    }

    /// <summary>
    /// Verifies that object IDs are correctly preserved and sequential
    /// </summary>
    public static void AssertObjectIdsArePreserved(List<DataObjectCoordinateModel> coordinates)
    {
        for (int i = 0; i < coordinates.Count; ++i)
        {
            Assert.Equal(i, coordinates[i].ObjectId);
        }
    }

    /// <summary>
    /// Verifies that all coordinates are at the origin (0, 0) within tolerance
    /// </summary>
    public static void AssertCoordinatesAreAtOrigin(
        List<DataObjectCoordinateModel> coordinates,
        double tolerance = 1e-10)
    {
        Assert.All(coordinates, coord =>
        {
            Assert.True(
                Math.Abs(coord.X) < tolerance,
                $"X coordinate {coord.X} is not within tolerance of origin"
            );

            Assert.True(
                Math.Abs(coord.Y) < tolerance,
                $"Y coordinate {coord.Y} is not within tolerance of origin"
            );
        });
    }

    /// <summary>
    /// Verifies that there is meaningful variance in the coordinates (not all identical)
    /// </summary>
    public static void AssertCoordinatesHaveVariance(
        List<DataObjectCoordinateModel> coordinates,
        double minVariance = 1e-10)
    {
        var xValues = coordinates.Select(c => c.X).ToList();
        var yValues = coordinates.Select(c => c.Y).ToList();

        var xRange = xValues.Max() - xValues.Min();
        var yRange = yValues.Max() - yValues.Min();

        Assert.True(
            xRange > minVariance || yRange > minVariance,
            "Coordinates do not have sufficient variance"
        );
    }

    /// <summary>
    /// Verifies that coordinates have non-zero variance along the first principal component
    /// </summary>
    public static void AssertFirstPrincipalComponentHasVariance(
        List<DataObjectCoordinateModel> coordinates,
        double minRange = 1e-10)
    {
        var xValues = coordinates.Select(c => c.X).ToList();
        var xRange = xValues.Max() - xValues.Min();

        Assert.True(
            xRange > minRange,
            $"First principal component range {xRange} is below minimum {minRange}"
        );
    }

    /// <summary>
    /// Verifies that at least one coordinate is not at the origin
    /// </summary>
    public static void AssertNotAllCoordinatesAreAtOrigin(
        List<DataObjectCoordinateModel> coordinates,
        double tolerance = 1e-10)
    {
        Assert.Contains(
            coordinates,
            c => Math.Abs(c.X) > tolerance || Math.Abs(c.Y) > tolerance
        );
    }
}
