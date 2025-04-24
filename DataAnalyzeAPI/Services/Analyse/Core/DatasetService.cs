using DataAnalyzeAPI.DAL.Repositories;
using DataAnalyzeAPI.Exceptions;
using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.DTOs.Analyse.Settings;
using DataAnalyzeAPI.Services.DataPreparation;

namespace DataAnalyzeAPI.Services.Analyse.Core;

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

    public DatasetModel NormalizeDataset(DatasetModel dataset)
    {
        return normalizer.Normalize(dataset);
    }

    public async Task<DatasetModel> GetPreparedDatasetAsync(
        long datasetId,
        List<ParameterSettingsDto>? parameterSettings)
    {
        var dataset = await repository.GetByIdAsync(datasetId);

        if (dataset == null)
        {
            throw new ResourceNotFoundException("Dataset", datasetId);
        }

        var mappedDataset = settingsMapper.Map(dataset, parameterSettings);

        return mappedDataset;
    }
}
