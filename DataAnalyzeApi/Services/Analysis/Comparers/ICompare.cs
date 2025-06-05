namespace DataAnalyzeApi.Services.Analysis.Comparers;

public interface ICompare
{
    double Compare(string valueA, string valueB, double maxRange);
}
