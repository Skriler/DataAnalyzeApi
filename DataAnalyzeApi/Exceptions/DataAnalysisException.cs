namespace DataAnalyzeApi.Exceptions;

public abstract class DataAnalysisException : Exception
{
    public virtual int StatusCode { get; } = StatusCodes.Status500InternalServerError;

    public virtual string ErrorTitle { get; } = "Data analyze exception";

    protected DataAnalysisException(string message, Exception? innerException = null)
        : base(message, innerException)
    { }
}
