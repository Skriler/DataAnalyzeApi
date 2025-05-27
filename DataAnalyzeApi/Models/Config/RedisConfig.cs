namespace DataAnalyzeApi.Models.Config;

public class RedisConfig
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string InstanceName { get; set; } = string.Empty;

    public int DefaultCacheDurationMinutes { get; set; }
}
