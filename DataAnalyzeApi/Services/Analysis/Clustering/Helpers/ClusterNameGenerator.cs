namespace DataAnalyzeApi.Services.Analysis.Clustering.Helpers;

public class ClusterNameGenerator
{
    private int counter = 0;

    public virtual string GenerateName(string prefix) => $"{prefix}_{counter++}";

    public virtual void Reset() => counter = 0;
}
