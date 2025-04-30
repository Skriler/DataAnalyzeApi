namespace DataAnalyzeApi.Exceptions.Vector;

public class VectorNullException : VectorValidationException
{
    public override string ErrorTitle { get; } = "Vector null error";

    public VectorNullException()
        : base("Parameter vector cannot be null.")
    { }
}
