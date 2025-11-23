using Misard.IQs.Domain.Common;
using Misard.IQs.Domain.Enums;

namespace Misard.IQs.Domain.Entities;

public class QuizSession : BaseEntity
{
    public int? UserId { get; set; }
    public User? User { get; set; }

    public int? TechnologyId { get; set; }
    public Technology? Technology { get; set; }

    public DifficultyLevel? DifficultyLevel { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    public int TotalQuestions { get; set; }
    public int? CorrectAnswers { get; set; }
    public decimal? ScorePercent { get; set; }

    public QuizStatus Status { get; set; } = QuizStatus.InProgress;

    public ICollection<QuizSessionAnswer> Answers { get; set; } = new List<QuizSessionAnswer>();
}
