namespace DataAnalyzeApi.Exceptions.Clustering;

public class ClusteringException : DataAnalysisException
{
    public override int StatusCode { get; } = StatusCodes.Status400BadRequest;

    public override string ErrorTitle { get; } = "Clustering Error";

    public ClusteringException(string message)
        : base(message)
    { }
}
