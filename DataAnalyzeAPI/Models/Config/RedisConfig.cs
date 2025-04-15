namespace DataAnalyzeAPI.Models.Config;

public class RedisConfig
{
    public string ConnectionString { get; set; } = string.Empty;

    public string InstanceName { get; set; } = string.Empty;

    public int DefaultCacheDurationMinutes { get; set; } = 20;
}
