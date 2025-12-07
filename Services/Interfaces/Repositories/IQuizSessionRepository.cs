using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Application.Interfaces.Repositories;

public interface IQuizSessionRepository
{
    Task<QuizSession> CreateSessionAsync(QuizSession session);
    Task<QuizSession?> GetByIdAsync(int sessionId);
    Task UpdateAsync(QuizSession session);

    Task<List<QuizSession>> GetSessionsByUserAsync(int userId);

}

