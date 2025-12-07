using Microsoft.EntityFrameworkCore;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Domain.Entities;
using Misard.IQs.Infrastructure.Persistence;

namespace Misard.IQs.Infrastructure.Repositories;

public class QuizSessionRepository : IQuizSessionRepository
{
    private readonly AppDbContext _db;

    public QuizSessionRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<QuizSession> CreateSessionAsync(QuizSession session)
    {
        _db.QuizSessions.Add(session);
        await _db.SaveChangesAsync();
        return session;
    }

    public async Task<QuizSession?> GetByIdAsync(int sessionId)
    {
        return await _db.QuizSessions
            .Include(s => s.Answers)
            .FirstOrDefaultAsync(s => s.Id == sessionId);
    }

    public async Task UpdateAsync(QuizSession session)
    {
        _db.QuizSessions.Update(session);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch(Exception ex)
        {

        }
    }

    public async Task<List<QuizSession>> GetSessionsByUserAsync(int userId)
    {
        return await _db.QuizSessions
            .Include(q => q.Technology)
            .Where(q => q.UserId == userId)
            .OrderByDescending(q => q.CreatedOn)
            .ToListAsync();
    }

    public async Task<QuizSession?> GetSessionWithTechAsync(int sessionId)
    {
        return await _db.QuizSessions
            .Include(s => s.Technology)
            .FirstOrDefaultAsync(s => s.Id == sessionId);
    }

    public async Task<List<QuizSession>> GetTopSessionsByTechnologyAsync(int techId, int limit)
    {
        return await _db.QuizSessions
            .Where(s => s.TechnologyId == techId && s.ScorePercent != null)
            .OrderByDescending(s => s.ScorePercent)
            .Take(limit)
            .Include(s => s.User)
            .ToListAsync();
    }

    public async Task<bool> DeleteSessionAsync(int sessionId)
    {
        var session = await _db.QuizSessions.FindAsync(sessionId);
        if (session == null) return false;

        _db.QuizSessions.Remove(session);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetAttemptCountAsync(int userId)
    {
        return await _db.QuizSessions.CountAsync(s => s.UserId == userId);
    }

    public async Task<QuizSession?> GetLatestSessionAsync(int userId)
    {
        return await _db.QuizSessions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedOn)
            .FirstOrDefaultAsync();
    }


}
