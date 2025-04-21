using DataAnalyzeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeAPI.DAL.Repositories;

public class DatasetRepository
{
    private readonly DataAnalyzeDbContext context;

    public DatasetRepository(DataAnalyzeDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Dataset>> GetAllAsync()
    {
        return await context.Datasets
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Dataset?> GetByIdAsync(long id)
    {
        return await context.Datasets
            .AsNoTracking()
            .Include(d => d.Parameters)
            .Include(d => d.Objects)
                .ThenInclude(o => o.Values)
            .FirstOrDefaultAsync(d => d.Id == id);
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
