namespace DataAnalyzeAPI.Models.Config.Clustering;

public static class KMeansConfig
{
    public static class MaxIterations
    {
        public const int Default = 1000;

        public const int MaxAllowed = 10000;

        public const int MinAllowed = 10;
    }

    public static class NumberOfClusters
    {
        public const int Default = 5;

        public const int MaxAllowed = 100;

        public const int MinAllowed = 2;
    }
}
