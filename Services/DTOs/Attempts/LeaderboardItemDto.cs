namespace Misard.IQs.Application.DTOs.Attempts;

public class LeaderboardItemDto
{
    public string UserName { get; set; } = string.Empty;
    public decimal ScorePercent { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime PlayedOn { get; set; }
}
