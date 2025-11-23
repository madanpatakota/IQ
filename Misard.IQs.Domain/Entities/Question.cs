using Misard.IQs.Domain.Common;
using Misard.IQs.Domain.Enums;

namespace Misard.IQs.Domain.Entities;

public class Question : BaseEntity
{
    public int TechnologyId { get; set; }
    public Technology Technology { get; set; } = null!;

    public DifficultyLevel DifficultyLevel { get; set; }

    public string QuestionText { get; set; } = null!;
    public string OptionA { get; set; } = null!;
    public string OptionB { get; set; } = null!;
    public string OptionC { get; set; } = null!;
    public string OptionD { get; set; } = null!;

    public char CorrectOption { get; set; }   // 'A','B','C','D'
    public string? Explanation { get; set; }

    public bool IsActive { get; set; } = true;
}

