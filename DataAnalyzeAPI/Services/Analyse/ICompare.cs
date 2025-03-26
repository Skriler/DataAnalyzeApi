namespace DataAnalyzeAPI.Services.Analyse;

public interface ICompare
{
    double Compare(string valueA, string valueB, double maxRange);
}
