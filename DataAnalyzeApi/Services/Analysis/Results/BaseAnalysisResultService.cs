using AutoMapper;
using DataAnalyzeApi.DAL.Repositories;
using DataAnalyzeApi.DAL.Repositories.Analysis;
using DataAnalyzeApi.Mappers.Analysis.Entities;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis;

namespace DataAnalyzeApi.Services.Analysis.Results;

public abstract class BaseAnalysisResultService<TEntity, TDto>(
    BaseEntityAnalysisMapper<TEntity, TDto> analysisMapper,
    BaseAnalysisRepository<TEntity> repository,
    DatasetRepository datasetRepository,
    IMapper mapper
    )
    where TEntity : AnalysisResult
    where TDto : BaseAnalysisResultDto
{
    protected readonly BaseEntityAnalysisMapper<TEntity, TDto> analysisMapper = analysisMapper;
    protected readonly BaseAnalysisRepository<TEntity> repository = repository;
    protected readonly DatasetRepository datasetRepository = datasetRepository;
    protected readonly IMapper mapper = mapper;

    /// <summary>
    /// Attaches related data objects from the dataset to the analysis result.
    /// </summary>
    protected abstract void AttachObjectsToAnalysisResult(TEntity entity, Dictionary<long, DataObject> datasetObjects);

    /// <summary>
    /// Retrieves the analysis result entity that matches the given hash from the repository.
    /// </summary>
    public async Task<TEntity?> GetByHashAsync(
        long datasetId,
        string requestHash,
        bool includeParameters)
    {
        return await repository.GetByHashAsync(
            datasetId,
            requestHash,
            includeParameters);
    }

    /// <summary>
    /// Retrieves and maps the analysis result entity to a DTO based on the request hash.
    /// </summary>
    public async Task<TDto?> GetDtoByHashAsync(
        long datasetId,
        string requestHash,
        bool includeParameters)
    {
        var result = await GetByHashAsync(
            datasetId,
            requestHash,
            includeParameters);

        if (result == null)
            return null;

        return analysisMapper.MapAnalysisResult(result, includeParameters);
    }

    /// <summary>
    /// Adds a new analysis result entity to the repository.
    /// </summary>
    public async Task AddAsync(TEntity entity)
    {
        await repository.AddAsync(entity);
    }

    /// <summary>
    /// Maps the DTO to an entity, attaches dataset objects, and saves the result.
    /// </summary>
    public async Task SaveDtoAsync(TDto dto, string requestHash)
    {
        var entity = mapper.Map<TEntity>(dto);
        entity.RequestHash = requestHash;

        await AttachDatasetObjectsAsync(entity);
        await AddAsync(entity);
    }

    /// <summary>
    /// Maps the DTO to an entity with additional configuration action, attaches dataset objects, and saves the result.
    /// </summary>
    public async Task SaveDtoAsync(
        TDto dto,
        string requestHash,
        Action<TEntity> configureEntity)
    {
        var entity = mapper.Map<TEntity>(dto);
        entity.RequestHash = requestHash;
        configureEntity(entity);

        await AttachDatasetObjectsAsync(entity);
        await AddAsync(entity);
    }

    /// <summary>
    /// Loads the dataset and attaches its objects to the given analysis result entity.
    /// </summary>
    protected async Task AttachDatasetObjectsAsync(TEntity entity)
    {
        var dataset = await datasetRepository.GetByIdAsync(entity.DatasetId, trackChanges: true);

        if (dataset == null)
            throw new InvalidOperationException($"Dataset with ID {entity.DatasetId} not found");

        var datasetObjects = dataset.Objects.ToDictionary(obj => obj.Id);
        entity.Dataset = dataset;

        AttachObjectsToAnalysisResult(entity, datasetObjects);
    }
}
