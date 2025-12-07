using Misard.IQs.Domain.Enums;

namespace Misard.IQs.Application.DTOs.Attempts;

public class ScorecardResultDto
{
    public int SessionId { get; set; }
    public string TechnologyName { get; set; } = string.Empty;
    public int DifficultyLevel { get; set; }

    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public decimal ScorePercent { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public double TimeTakenSeconds { get; set; }

    public QuizStatus Status { get; set; }
}
