namespace DataAnalyzeApi.Exceptions;

public class TypeNotFoundException : DataAnalysisException
{
    public override int StatusCode { get; } = StatusCodes.Status404NotFound;

    public override string ErrorTitle { get; } = "Metric Type Not Found";

    public string TypeName { get; }

    public TypeNotFoundException(string typeName)
        : base($"Type '{typeName}' not found.")
    {
        TypeName = typeName;
    }
}
