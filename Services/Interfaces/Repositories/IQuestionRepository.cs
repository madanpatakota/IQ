using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Application.Interfaces.Repositories;

public interface IQuestionRepository
{
    Task<List<Question>> GetRandomQuestionsAsync(int technologyId, int count, string? difficulty);

    Task<Question?> GetByIdAsync(int id);
}
