namespace DataAnalyzeApi.Exceptions.Vector;

public abstract class VectorValidationException(
    string message
    ) : DataAnalysisException(message)
{
    public override int StatusCode { get; } = StatusCodes.Status400BadRequest;

    public override string ErrorTitle { get; } = "Vector validation error";
}
