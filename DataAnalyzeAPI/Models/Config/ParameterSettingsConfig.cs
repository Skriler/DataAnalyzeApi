namespace DataAnalyzeApi.Models.Config;

public static class ParameterSettingsConfig
{
    public static class Activity
    {
        /// <summary>
        /// Default value of activity status setting.
        /// </summary>
        public const bool Default = true;
    }

    public static class Weight
    {
        /// <summary>
        /// Default value of weight setting.
        /// </summary>
        public const double Default = 1.0;

        /// <summary>
        /// Max value of weight setting.
        /// </summary>
        public const double MaxAllowed = 10.0;

        /// <summary>
        /// Min value of weight setting.
        /// </summary>
        public const double MinAllowed = 0.1;
    }
}
