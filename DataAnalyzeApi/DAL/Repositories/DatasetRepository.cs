using DataAnalyzeApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.DAL.Repositories;

public class DatasetRepository
{
    private readonly DataAnalyzeDbContext context;

    public DatasetRepository(DataAnalyzeDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Dataset>> GetAllAsync(bool trackChanges = false)
    {
        var query = context.Datasets.AsQueryable();

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<Dataset?> GetByIdAsync(long id, bool trackChanges = false)
    {
        var query = context.Datasets
            .Include(d => d.Parameters)
            .Include(d => d.Objects)
                .ThenInclude(o => o.Values)
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
}
