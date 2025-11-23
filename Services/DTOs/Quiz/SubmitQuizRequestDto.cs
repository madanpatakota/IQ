namespace Misard.IQs.Application.DTOs.Quiz;

public class SubmitQuizRequestDto
{
    public int SessionId { get; set; }
    public List<SubmitAnswerDto> Answers { get; set; } = new();
}

public class SubmitAnswerDto
{
    public int QuestionId { get; set; }
    public char? SelectedOption { get; set; }
}
