using Misard.IQs.Application.DTOs.Questions;

namespace Misard.IQs.Application.Interfaces.Services;

public interface IQuestionService
{
    Task<List<QuestionDto>> GetQuestionsByTechAsync(int technologyId);
}

