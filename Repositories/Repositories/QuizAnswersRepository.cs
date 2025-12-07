using Microsoft.EntityFrameworkCore;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Domain.Entities;
using Misard.IQs.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _db.QuizSessionAnswers
                .Include(a => a.Question)
                .Where(a => a.SessionId == sessionId)
                .ToListAsync();
        }
    }

}
