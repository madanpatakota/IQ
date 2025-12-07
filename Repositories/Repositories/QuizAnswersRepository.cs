using Microsoft.EntityFrameworkCore;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Domain.Entities;
using Misard.IQs.Infrastructure.Persistence;


namespace Misard.IQs.Infrastructure.Repositories
{
    public class QuizAnswersRepository : IQuizAnswersRepository
    {
        private readonly AppDbContext _db;

        public QuizAnswersRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<QuizSessionAnswer>> GetBySessionIdAsync(int sessionId)
        {

            var testDAta1 = await _db.QuizSessionAnswers.ToListAsync();

            return await _db.QuizSessionAnswers
                .Include(a => a.Question)
                .Where(a => a.SessionId == sessionId)
                .ToListAsync();
        }
    }

}
