namespace DataAnalyzeApi.Exceptions.Vector;

public class EmptyVectorException : VectorValidationException
{
    public override string ErrorTitle { get; } = "Empty vector error";

    public EmptyVectorException()
        : base("Vectors cannot be empty.")
    { }
}
