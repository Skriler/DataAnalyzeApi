namespace DataAnalyzeApi.Models.Config.Clustering;

public static class AgglomerativeConfig
{
    public static class Threshold
    {
        public const double Default = 0.2;

        public const double MaxAllowed = 1.0;

        public const double MinAllowed = 0.01;
    }
}
