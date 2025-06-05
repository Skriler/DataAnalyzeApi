namespace DataAnalyzeApi.Exceptions.Vector;

public class VectorLengthMismatchException : VectorValidationException
{
    public override string ErrorTitle { get; } = "Vector length mismatch error";

    public VectorLengthMismatchException()
        : base("Parameter vectors must have the same length.")
    { }
}
