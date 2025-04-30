namespace DataAnalyzeApi.Services.Analyse.Comparers;

public interface ICompare
{
    double Compare(string valueA, string valueB, double maxRange);
}
