using DataAnalyzeApi.DAL.Repositories;
using DataAnalyzeApi.Exceptions;
using DataAnalyzeApi.Mappers.Entities;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Services.Normalizers;

namespace DataAnalyzeApi.Services.Analysis.Core;

public class DatasetService(
    DatasetRepository repository,
    DatasetSettingsMapper datasetSettingsMapper,
    DatasetNormalizer datasetNormalizer)
{
    private readonly DatasetRepository repository = repository;
    private readonly DatasetSettingsMapper settingsMapper = datasetSettingsMapper;
    private readonly DatasetNormalizer normalizer = datasetNormalizer;

    /// <summary>
    /// Retrieves a dataset by ID, applies parameter settings if provided, and maps it to a model.
    /// </summary>
    public async Task<DatasetModel> GetPreparedDatasetAsync(
        long datasetId,
        List<ParameterSettingsDto>? parameterSettings)
    {
        var dataset = await repository.GetByIdAsync(datasetId);

        if (dataset == null)
            throw new ResourceNotFoundException(nameof(Dataset), datasetId);

        return settingsMapper.Map(dataset, parameterSettings);
    }

    /// <summary>
    /// Retrieves a prepared dataset and applies normalization.
    /// </summary>
    public async Task<DatasetModel> GetPreparedNormalizedDatasetAsync(
        long datasetId,
        List<ParameterSettingsDto>? parameterSettings)
    {
        var dataset = await GetPreparedDatasetAsync(datasetId, parameterSettings);

        return normalizer.Normalize(dataset);
    }
}
