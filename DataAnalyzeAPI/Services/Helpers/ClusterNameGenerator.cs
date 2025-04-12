namespace DataAnalyzeAPI.Services.Helpers;

public class ClusterNameGenerator
{
    private int counter = 0;

    public string GenerateName(string prefix) => $"{prefix}_{counter++}";

    public void Reset() => counter = 0;
}
