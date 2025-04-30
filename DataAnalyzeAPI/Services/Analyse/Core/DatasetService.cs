using DataAnalyzeApi.DAL.Repositories;
using DataAnalyzeApi.Exceptions;
using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings;
using DataAnalyzeApi.Services.DataPreparation;

namespace DataAnalyzeApi.Services.Analyse.Core;

public class DatasetService
{
    private readonly DatasetRepository repository;
    private readonly DatasetSettingsMapper settingsMapper;
    private readonly DatasetNormalizer normalizer;

    public DatasetService(
        DatasetRepository repository,
        DatasetSettingsMapper datasetSettingsMapper,
        DatasetNormalizer datasetNormalizer)
    {
        this.repository = repository;
        settingsMapper = datasetSettingsMapper;
        normalizer = datasetNormalizer;
    }

    /// <summary>
    /// Retrieves a dataset by ID, applies parameter settings if provided, and maps it to a model.
    /// </summary>
    public async Task<DatasetModel> GetPreparedDatasetAsync(
        long datasetId,
        List<ParameterSettingsDto>? parameterSettings)
    {
        var dataset = await repository.GetByIdAsync(datasetId);

        if (dataset == null)
        {
            throw new ResourceNotFoundException("Dataset", datasetId);
        }

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
