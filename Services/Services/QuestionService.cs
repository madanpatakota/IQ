using Misard.IQs.Application.DTOs.Questions;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Application.Interfaces.Services;

namespace Misard.IQs.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepo;

    public QuestionService(IQuestionRepository questionRepo)
    {
        _questionRepo = questionRepo;
    }

    public async Task<List<QuestionDto>> GetQuestionsByTechAsync(int technologyId)
    {
        // For now, reuse GetRandomQuestionsAsync with a large count
        var questions = await _questionRepo.GetRandomQuestionsAsync(technologyId, 200, null);

        return questions.Select(q => new QuestionDto
        {
            Id = q.Id,
            QuestionText = q.QuestionText,
            OptionA = q.OptionA,
            OptionB = q.OptionB,
            OptionC = q.OptionC,
            OptionD = q.OptionD
        }).ToList();
    }
}
