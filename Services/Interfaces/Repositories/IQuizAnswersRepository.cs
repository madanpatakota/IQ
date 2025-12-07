using Misard.IQs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misard.IQs.Application.Interfaces.Repositories
{
    public interface IQuizAnswersRepository
    {
        Task<List<QuizSessionAnswer>> GetBySessionIdAsync(int sessionId);
    }
}
