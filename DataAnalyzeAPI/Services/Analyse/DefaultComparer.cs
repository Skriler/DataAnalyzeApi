namespace DataAnalyzeAPI.Services.Analyse;

public class DefaultComparer : ICompare
{
    public double Compare(string valueA, string valueB, double maxRange)
    {
        if (double.TryParse(valueA, out var numberA) &&
            double.TryParse(valueB, out var numberB))
        {
            return CompareNumerical(numberA, numberB, maxRange);
        }

        return CompareCategorical(valueA, valueB);
    }

    private static double CompareNumerical(double valueA, double valueB, double maxRange)
    {
        var difference = Math.Abs(valueA - valueB);
        var similarity = 1 - (difference / maxRange);
        return similarity;
    }

    private static double CompareCategorical(string valueA, string valueB)
    {
        if (string.IsNullOrWhiteSpace(valueA) || string.IsNullOrWhiteSpace(valueB))
            return 0;

        var valuesA = GetSplitValues(valueA);
        var valuesB = GetSplitValues(valueB);

        var intersection = valuesA.Intersect(valuesB).Count();
        var union = valuesA.Union(valuesB).Count();

        return (double)intersection / union;
    }

    private static string[] GetSplitValues(string values)
    {
        return values.Split(',')
            .Select(v => v.Trim())
            .Order()
            .ToArray();
    }
}
