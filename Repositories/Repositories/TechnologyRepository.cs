using Microsoft.EntityFrameworkCore;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Domain.Entities;
using Misard.IQs.Infrastructure.Persistence;

namespace Misard.IQs.Infrastructure.Repositories;

public class TechnologyRepository : ITechnologyRepository
{
    private readonly AppDbContext _db;

    public TechnologyRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Technology>> GetAllAsync()
    {
        return await _db.Technologies
            .Where(t => t.IsActive)
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Name)
            .ToListAsync();
    }
}
