using Misard.IQs.Application.DTOs.Quiz;
using Misard.IQs.Application.Exceptions;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Application.Interfaces.Services;
using Misard.IQs.Domain.Entities;
using Misard.IQs.Domain.Enums;

namespace Misard.IQs.Application.Services;

public class QuizService : IQuizService
{
    private readonly IQuestionRepository _questionRepo;
    private readonly IQuizSessionRepository _sessionRepo;

    public QuizService(
        IQuestionRepository questionRepo,
        IQuizSessionRepository sessionRepo)
    {
        _questionRepo = questionRepo;
        _sessionRepo = sessionRepo;
    }

    public async Task<StartQuizResponseDto> StartQuizAsync(StartQuizRequestDto request)
    {
        if (request.QuestionCount <= 0)
        {
            throw new ValidationException("Question count must be greater than zero.");
        }

        // Get random questions
        var questions = await _questionRepo.GetRandomQuestionsAsync(
            request.TechnologyId,
            request.QuestionCount,
            request.DifficultyLevel);

        if (questions.Count == 0)
        {
            throw new NotFoundException("No questions found for the selected criteria.");
        }

        var session = new QuizSession
        {
            TechnologyId = request.TechnologyId,
            DifficultyLevel = ParseDifficulty(request.DifficultyLevel),
            StartedAt = DateTime.UtcNow,
            TotalQuestions = questions.Count,
            Status = QuizStatus.InProgress
        };

        // Create session in DB
        session = await _sessionRepo.CreateSessionAsync(session);

        var expiresAt = session.StartedAt.AddMinutes(15);   // 15-minute session rule

        var questionDtos = questions.Select(q => new DTOs.Questions.QuestionDto
        {
            Id = q.Id,
            QuestionText = q.QuestionText,
            OptionA = q.OptionA,
            OptionB = q.OptionB,
            OptionC = q.OptionC,
            OptionD = q.OptionD
        }).ToList();

        return new StartQuizResponseDto
        {
            SessionId = session.Id,
            Questions = questionDtos,
            ExpiresAt = expiresAt
        };
    }

    public async Task<QuizResultDto> SubmitQuizAsync(SubmitQuizRequestDto request)
    {
        var session = await _sessionRepo.GetByIdAsync(request.SessionId);
        if (session == null)
        {
            throw new NotFoundException("Quiz session not found.");
        }

        // Check if already completed/expired
        if (session.Status == QuizStatus.Completed || session.Status == QuizStatus.Expired)
        {
            return BuildResultDto(session);
        }

        // Check 15-minute window
        var now = DateTime.UtcNow;
        if (now > session.StartedAt.AddMinutes(15))
        {
            session.Status = QuizStatus.Expired;
            session.EndedAt = now;
            await _sessionRepo.UpdateAsync(session);
            throw new BusinessException("Time is over. Session has expired.");
        }

        // Map answers (simple version: we fetch questions one by one)
        int correctCount = 0;

        foreach (var answer in request.Answers)
        {
            // In a real system, better to bulk load; here for clarity.
            var question = await _questionRepo.GetByIdAsync(answer.QuestionId);
            if (question == null) continue;

            bool isCorrect = answer.SelectedOption.HasValue &&
                             char.ToUpperInvariant(answer.SelectedOption.Value) == question.CorrectOption;

            if (isCorrect) correctCount++;

            session.Answers.Add(new QuizSessionAnswer
            {
                QuestionId = answer.QuestionId,
                SelectedOption = answer.SelectedOption.HasValue
                    ? char.ToUpperInvariant(answer.SelectedOption.Value)
                    : null,
                IsCorrect = isCorrect
            });
        }

        session.CorrectAnswers = correctCount;
        session.ScorePercent = session.TotalQuestions == 0
            ? 0
            : Math.Round((decimal)correctCount * 100 / session.TotalQuestions, 2);

        session.Status = QuizStatus.Completed;
        session.EndedAt = now;

        await _sessionRepo.UpdateAsync(session);

        return BuildResultDto(session);
    }

    public async Task<QuizResultDto> GetResultAsync(int sessionId)
    {
        var session = await _sessionRepo.GetByIdAsync(sessionId);
        if (session == null)
        {
            throw new NotFoundException("Quiz session not found.");
        }

        return BuildResultDto(session);
    }

    private static DifficultyLevel? ParseDifficulty(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        if (Enum.TryParse<DifficultyLevel>(input, ignoreCase: true, out var value))
        {
            return value;
        }

        return null;
    }

    private static QuizResultDto BuildResultDto(QuizSession session)
    {
        var scorePercent = session.ScorePercent ?? 0;
        var statusText = session.Status.ToString();

        string message;

        if (session.Status == QuizStatus.Expired)
        {
            message = "Your 15-minute time limit is over. Don’t worry, we are here to help you. Reach out to Misard for free guidance and mock interviews.";
        }
        else if (scorePercent < 40)
        {
            message = "You may not have cleared the test this time, but don’t get discouraged. We can help you improve with free guidance and practice material. Contact Misard – your success is our priority.";
        }
        else if (scorePercent < 70)
        {
            message = "Good attempt! You are on the right track. With a bit more practice, you can easily reach an advanced level. Keep going – and reach out to Misard if you want structured support.";
        }
        else
        {
            message = "Excellent work! Your fundamentals are strong. You’re ready to face real interviews. If you want advanced guidance or mock interviews, Misard is here to support you.";
        }

        return new QuizResultDto
        {
            SessionId = session.Id,
            TotalQuestions = session.TotalQuestions,
            CorrectAnswers = session.CorrectAnswers ?? 0,
            ScorePercent = scorePercent,
            Status = statusText,
            Message = message
        };
    }
}
