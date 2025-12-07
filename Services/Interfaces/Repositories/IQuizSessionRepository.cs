using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Application.Interfaces.Repositories;

public interface IQuizSessionRepository
{
    Task<QuizSession> CreateSessionAsync(QuizSession session);
    Task<QuizSession?> GetByIdAsync(int sessionId);
    Task UpdateAsync(QuizSession session);

    Task<List<QuizSession>> GetSessionsByUserAsync(int userId);

    Task<QuizSession?> GetSessionWithTechAsync(int sessionId);
    Task<List<QuizSession>> GetTopSessionsByTechnologyAsync(int techId, int limit);
    Task<bool> DeleteSessionAsync(int sessionId);
    Task<int> GetAttemptCountAsync(int userId);
    Task<QuizSession?> GetLatestSessionAsync(int userId);


}

