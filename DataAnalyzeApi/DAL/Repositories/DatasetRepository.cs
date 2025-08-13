using DataAnalyzeApi.Models.DTOs.Common;
using DataAnalyzeApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.DAL.Repositories;

public class DatasetRepository(DataAnalyzeDbContext context)
{
    private readonly DataAnalyzeDbContext context = context;

    /// <summary>
    /// Retrieves all datasets from the database.
    /// </summary>
    public async Task<List<Dataset>> GetAllAsync(bool trackChanges = false)
    {
        var query = GetDatasetsWithIncludes();

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    /// <summary>
    /// Retrieves all datasets with pagination support.
    /// </summary>
    public async Task<PaginationResult<Dataset>> GetAllPagedAsync(
        PaginationRequest request,
        bool trackChanges = false)
    {
        var query = GetDatasetsWithIncludes();

        if (request.FromDate.HasValue)
            query = query.Where(d => d.CreatedAt >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(d => d.CreatedAt <= request.ToDate.Value);

        if (!trackChanges)
            query = query.AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PaginationResult<Dataset>
        {
            Data = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };
    }

    /// <summary>
    /// Retrieves a specific dataset by its ID.
    /// </summary>
    public async Task<Dataset?> GetByIdAsync(long id, bool trackChanges = false)
    {
        var query = GetDatasetsWithIncludes()
            .Where(d => d.Id == id);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Adds a new dataset to the database.
    /// </summary>
    public async Task AddAsync(Dataset dataset)
    {
        context.Datasets.Add(dataset);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Removes a dataset from the database.
    /// Due to cascade delete configuration, this will also remove all related parameters, objects, values and analysis results.
    /// </summary>
    public async Task DeleteAsync(Dataset dataset)
    {
        context.Datasets.Remove(dataset);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Creates a query for datasets with all necessary related data included.
    /// Includes parameters, objects, and their parameter values for complete data retrieval.
    /// </summary>
    private IQueryable<Dataset> GetDatasetsWithIncludes()
    {
        return context.Datasets
            .Include(d => d.Parameters)
            .Include(d => d.Objects)
                .ThenInclude(o => o.Values);
    }
}
