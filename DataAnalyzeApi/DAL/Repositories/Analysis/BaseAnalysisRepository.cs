using DataAnalyzeApi.Models.DTOs.Common;
using DataAnalyzeApi.Models.Entities.Analysis;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.DAL.Repositories.Analysis;

public abstract class BaseAnalysisRepository<TAnalysisResult>(DataAnalyzeDbContext context)
    where TAnalysisResult : AnalysisResult
{
    protected readonly DataAnalyzeDbContext context = context;

    /// <summary>
    /// Get a DbSet for a specific analysis type.
    /// </summary>
    protected abstract DbSet<TAnalysisResult> GetDbSet();

    /// <summary>
    /// Creates query with includes. Adds parameter values if includeParameters is true.
    /// </summary>
    protected abstract IQueryable<TAnalysisResult> GetAnalysisResultsWithIncludes(bool includeParameters = false);

    /// <summary>
    /// Retrieves all analysis results. Includes parameters if flag is true.
    /// </summary>
    public virtual async Task<List<TAnalysisResult>> GetAllAsync(
        bool includeParameters = false,
        bool trackChanges = false)
    {
        var query = GetAnalysisResultsWithIncludes(includeParameters);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves paginated analysis results with caching. Includes parameters if flag is true.
    /// </summary>
    public virtual async Task<PaginationResult<TAnalysisResult>> GetPagedAsync(
        PaginationRequest request,
        bool includeParameters = false,
        bool trackChanges = false)
    {
        var analysisType = typeof(TAnalysisResult).Name.Replace("AnalysisResult", "").ToLower();

        // TODO: Try to get from cache first

        var query = GetAnalysisResultsWithIncludes(includeParameters);

        if (request.FromDate.HasValue)
            query = query.Where(a => a.CreatedAt >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(a => a.CreatedAt <= request.ToDate.Value);

        if (!trackChanges)
            query = query.AsNoTracking();

        var totalCount = await query.CountAsync();

        var data = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var result = new PaginationResult<TAnalysisResult>
        {
            Data = data,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        // TODO: Cache the result

        return result;
    }

    /// <summary>
    /// Retrieves all analysis results for a dataset. Includes parameters if flag is true.
    /// </summary>
    public virtual async Task<List<TAnalysisResult>> GetAllByDatasetAsync(
        long datasetId,
        bool includeParameters = false,
        bool trackChanges = false)
    {
        var query = GetAnalysisResultsWithIncludes(includeParameters)
            .Where(a => a.DatasetId == datasetId);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves an analysis result by its unique request hash for a dataset.
    /// Includes parameters if flag is true.
    /// </summary>
    public virtual async Task<TAnalysisResult?> GetByHashAsync(
        long datasetId,
        string requestHash,
        bool includeParameters = false,
        bool trackChanges = false)
    {
        var query = GetAnalysisResultsWithIncludes(includeParameters)
            .Where(a => a.DatasetId == datasetId && a.RequestHash == requestHash);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Adds a new analysis result to the database.
    /// </summary>
    public virtual async Task AddAsync(TAnalysisResult analysisResult)
    {
        GetDbSet().Add(analysisResult);
        await context.SaveChangesAsync();
    }
}
