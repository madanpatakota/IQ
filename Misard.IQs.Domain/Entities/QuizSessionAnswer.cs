using Misard.IQs.Domain.Common;

namespace Misard.IQs.Domain.Entities;

public class QuizSessionAnswer : BaseEntity
{
    public int SessionId { get; set; }
    public QuizSession Session { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public char? SelectedOption { get; set; }   // what student chose
    public bool? IsCorrect { get; set; }
}
