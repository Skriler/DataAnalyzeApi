using AutoMapper;
using DataAnalyzeApi.DAL.Repositories;
using DataAnalyzeApi.DAL.Repositories.Analysis;
using DataAnalyzeApi.Mappers.Analysis.Entities;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Services.Analysis.Results;

public class ClusteringAnalysisResultService: BaseAnalysisResultService<ClusteringAnalysisResult, ClusteringAnalysisResultDto>
{
    public ClusteringAnalysisResultService(
        ClusteringEntityAnalysisMapper analysisMapper,
        ClusteringAnalysisResultRepository repository,
        DatasetRepository datasetRepository,
        IMapper mapper)
        : base(analysisMapper, repository, datasetRepository, mapper)
    { }

    /// <summary>
    /// Maps the DTO to an entity, attaches dataset objects, and saves the result.
    /// </summary>
    public virtual async Task SaveDtoAsync(
        ClusteringAnalysisResultDto dto,
        string requestHash,
        ClusteringAlgorithm algorithm)
    {
        await SaveDtoAsync(dto, requestHash, entity => entity.Algorithm = algorithm);
    }

    /// <summary>
    /// Attaches the actual dataset objects to each cluster in the analysis result and links coordinates.
    /// </summary>
    protected override void AttachObjectsToAnalysisResult(
        ClusteringAnalysisResult entity,
        Dictionary<long, DataObject> datasetObjects)
    {
        var objectCoordinates = entity.ObjectCoordinates.ToDictionary(obj => obj.ObjectId);

        foreach (var cluster in entity.Clusters)
        {
            ProcessClusterObjects(
                cluster,
                datasetObjects,
                objectCoordinates,
                entity.DatasetId);
        }
    }

    /// <summary>
    /// Replaces object IDs with actual objects and links coordinates.
    /// </summary>
    private static void ProcessClusterObjects(
        Cluster cluster,
        Dictionary<long, DataObject> datasetObjects,
        Dictionary<long, DataObjectCoordinate> objectCoordinates,
        long datasetId)
    {
        var objectIds = cluster.Objects.ConvertAll(obj => obj.Id);
        cluster.Objects.Clear();

        foreach (var objectId in objectIds)
        {
            var realObject = GetAndValidateDataObject(objectId, datasetObjects, datasetId);
            cluster.Objects.Add(realObject);

            LinkObjectCoordinate(realObject, objectCoordinates);
        }
    }

    /// <summary>
    /// Retrieves and validates a DataObject by ID.
    /// </summary>
    private static DataObject GetAndValidateDataObject(
        long objectId,
        Dictionary<long, DataObject> datasetObjects,
        long datasetId)
    {
        if (!datasetObjects.TryGetValue(objectId, out var realObject))
        {
            throw new InvalidOperationException($"Object with ID {objectId} not found in dataset {datasetId}");
        }

        return realObject;
    }

    /// <summary>
    /// Links a DataObject to DataObjectCoordinate.
    /// </summary>
    private static void LinkObjectCoordinate(
        DataObject obj,
        Dictionary<long, DataObjectCoordinate> objectCoordinates)
    {
        if (!objectCoordinates.TryGetValue(obj.Id, out var objectCoordinate))
        {
            throw new InvalidOperationException($"Object coordinate with object ID {obj.Id} not found in result");
        }

        objectCoordinate.Object = obj;
    }
}
