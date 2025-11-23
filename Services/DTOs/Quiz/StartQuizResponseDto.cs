using Misard.IQs.Application.DTOs.Questions;

namespace Misard.IQs.Application.DTOs.Quiz;

public class StartQuizResponseDto
{
    public int SessionId { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
    public DateTime ExpiresAt { get; set; }
}
