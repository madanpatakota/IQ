using Misard.IQs.Application.DTOs.Attempts;
using Misard.IQs.Application.DTOs.Quiz;

namespace Misard.IQs.Application.Interfaces.Services;

public interface IQuizService
{
    Task<StartQuizResponseDto> StartQuizAsync(StartQuizRequestDto request);
    Task<QuizResultDto> SubmitQuizAsync(SubmitQuizRequestDto request);
    Task<QuizResultDto> GetResultAsync(int sessionId);

    Task<List<AttemptListItemDto>> GetAttemptsByUserAsync(int userId);
    Task<List<AttemptDetailDto>> GetAttemptDetailsAsync(int sessionId);

}
