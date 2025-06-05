using DataAnalyzeApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.DAL.Repositories;

public class DatasetRepository(DataAnalyzeDbContext context)
{
    private readonly DataAnalyzeDbContext context = context;

    public async Task<List<Dataset>> GetAllAsync(bool trackChanges = false)
    {
        var query = GetDatasetsWithIncludes();

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<Dataset?> GetByIdAsync(long id, bool trackChanges = false)
    {
        var query = GetDatasetsWithIncludes()
            .Where(d => d.Id == id);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task AddAsync(Dataset dataset)
    {
        context.Datasets.Add(dataset);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Dataset dataset)
    {
        context.Datasets.Remove(dataset);
        await context.SaveChangesAsync();
    }

    private IQueryable<Dataset> GetDatasetsWithIncludes()
    {
        return context.Datasets
            .Include(d => d.Parameters)
            .Include(d => d.Objects)
                .ThenInclude(o => o.Values);
    }
}
