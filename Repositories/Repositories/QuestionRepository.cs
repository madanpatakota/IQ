using Microsoft.EntityFrameworkCore;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Domain.Entities;
using Misard.IQs.Domain.Enums;              // ⬅️ important
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

        if (!string.IsNullOrWhiteSpace(difficulty) &&
            Enum.TryParse<DifficultyLevel>(difficulty, ignoreCase: true, out var diffEnum))
        {
            // ✅ enum-to-enum comparison (translatable to SQL)
            query = query.Where(q => q.DifficultyLevel == diffEnum);
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
