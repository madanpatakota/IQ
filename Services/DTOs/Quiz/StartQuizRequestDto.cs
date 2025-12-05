namespace Misard.IQs.Application.DTOs.Quiz;

public class StartQuizRequestDto
{
    public int TechnologyId { get; set; }
    public string DifficultyLevel { get; set; } = null!;
    public int QuestionCount { get; set; } = 10;   // default 10

    public int UserId { get; set; }
}

