namespace DataAnalyzeAPI.Exceptions;

public abstract class DataAnalyzeException : Exception
{
    public virtual int StatusCode { get; } = StatusCodes.Status500InternalServerError;

    public virtual string ErrorTitle { get; } = "Data analyze exception";

    protected DataAnalyzeException(string message, string errorTitle, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorTitle = errorTitle ?? "Data analyze exception";
    }
}
