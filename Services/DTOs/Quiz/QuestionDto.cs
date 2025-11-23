namespace Misard.IQs.Application.DTOs.Questions;

public class QuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = null!;
    public string OptionA { get; set; } = null!;
    public string OptionB { get; set; } = null!;
    public string OptionC { get; set; } = null!;
    public string OptionD { get; set; } = null!;
}

