namespace DataAnalyzeAPI.Exceptions;

public class ResourceNotFoundException : DataAnalyzeException
{
    public override int StatusCode { get; } = StatusCodes.Status404NotFound;

    public string ResourceType { get; }

    public long ResourceId { get; }

    public ResourceNotFoundException(string resourceType, long resourceId)
        : base($"{resourceType} with ID {resourceId} not found.", "Resource not found")
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
    }
}
