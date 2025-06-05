namespace DataAnalyzeApi.Models.Config.Clustering;

public static class DBSCANConfig
{
    public static class Epsilon
    {
        public const double Default = 0.2;

        public const double MaxAllowed = 1.0;

        public const double MinAllowed = 0.01;
    }

    public static class MinPoints
    {
        public const int Default = 2;

        public const int MaxAllowed = 20;

        public const int MinAllowed = 1;
    }
}
