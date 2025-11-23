namespace Misard.IQs.Application.DTOs.Quiz;

public class QuizResultDto
{
    public int SessionId { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public decimal ScorePercent { get; set; }
    public string Status { get; set; } = null!;
    public string Message { get; set; } = null!;  // Motivation, Pass/Fail msg
}

