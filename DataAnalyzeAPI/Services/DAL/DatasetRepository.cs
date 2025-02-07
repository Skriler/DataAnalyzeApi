using DataAnalyzeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeAPI.Services.DAL;

public class DatasetRepository
{
    private readonly DataAnalyzeDbContext context;

    public DatasetRepository(DataAnalyzeDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Dataset dataset)
    {
        context.Datasets.Add(dataset);
        await context.SaveChangesAsync();
    }

    public async Task<Dataset?> GetById(int id)
    {
        return await context.Datasets
            .Include(d => d.Parameters)
            .Include(d => d.Objects)
                .ThenInclude(o => o.Values)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
}
