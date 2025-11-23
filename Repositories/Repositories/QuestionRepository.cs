using Microsoft.EntityFrameworkCore;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Domain.Entities;
using Misard.IQs.Infrastructure.Persistence;

namespace Misard.IQs.Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly AppDbContext _db;

    public QuestionRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Question>> GetRandomQuestionsAsync(int technologyId, int count, string? difficulty)
    {
        var query = _db.Questions
            .Where(q => q.TechnologyId == technologyId && q.IsActive);

        if (!string.IsNullOrWhiteSpace(difficulty))
        {
            // difficulty stored as enum (Fresher/Intermediate/Advanced)
            query = query.Where(q => q.DifficultyLevel.ToString() == difficulty);
        }

        return await query
            .OrderBy(_ => Guid.NewGuid())
            .Take(count)
            .ToListAsync();
    }

    public async Task<Question?> GetByIdAsync(int id)
    {
        return await _db.Questions.FindAsync(id);
    }
}
