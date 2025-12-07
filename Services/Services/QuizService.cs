using Misard.IQs.Application.DTOs.Attempts;
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
    private readonly IQuizAnswersRepository _quizAnswersRepository;


    public QuizService(
        IQuestionRepository questionRepo,
        IQuizSessionRepository sessionRepo,
        IQuizAnswersRepository quizAnswersRepository)
    {
        _questionRepo = questionRepo;
        _sessionRepo = sessionRepo;
        _quizAnswersRepository = quizAnswersRepository;
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
            Status = QuizStatus.InProgress,
            UserId = request.UserId
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
            OptionD = q.OptionD,
            CorrectOption = q.CorrectOption,
            //Explanation = q.Explanation,
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
            try
            {
                await _sessionRepo.UpdateAsync(session);
            }
            catch(Exception ex)
            {

            }
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

    // -------------------------------
    // 1️⃣ Get Attempts List (For UI)
    // -------------------------------
    public async Task<List<AttemptListItemDto>> GetAttemptsByUserAsync(int userId)
    {
        var sessions = await _sessionRepo.GetSessionsByUserAsync(userId);

        return sessions.Select(s => new AttemptListItemDto
        {
            SessionId = s.Id,
            TechnologyName = s.Technology?.Name ?? "",
            DifficultyLevel = (int)(s.DifficultyLevel ?? 0),
            TotalQuestions = s.TotalQuestions,
            CorrectAnswers = s.CorrectAnswers ?? 0,
            ScorePercent = s.ScorePercent ?? 0,
            CreatedOn = s.CreatedOn,
            Status = s.Status
        }).ToList();
    }


    // ---------------------------------------
    // 2️⃣ Get Attempt Details (Question + Answer)
    // ---------------------------------------
    public async Task<List<AttemptDetailDto>> GetAttemptDetailsAsync(int sessionId)
    {
        var answers = await _quizAnswersRepository.GetBySessionIdAsync(sessionId);

        return answers.Select(a => new AttemptDetailDto
        {
            QuestionId = a.Question.Id,
            QuestionText = a.Question.QuestionText,
            OptionA = a.Question.OptionA,
            OptionB = a.Question.OptionB,
            OptionC = a.Question.OptionC,
            OptionD = a.Question.OptionD,

            SelectedOption = a.SelectedOption?.ToString() ?? "",
            CorrectOption = a.Question.CorrectOption.ToString(),
            IsCorrect = a.IsCorrect ?? false,
            Explanation = a.Question.Explanation

        }).ToList();
    }


    public async Task<ScorecardResultDto> GetScorecardAsync(int sessionId)
    {
        var session = await _sessionRepo.GetSessionWithTechAsync(sessionId);
        if (session == null)
            throw new Exception("Session not found");

        return new ScorecardResultDto
        {
            SessionId = session.Id,
            TechnologyName = session.Technology?.Name ?? "",
            DifficultyLevel = (int)(session.DifficultyLevel ?? 0),
            TotalQuestions = session.TotalQuestions,
            CorrectAnswers = session.CorrectAnswers ?? 0,
            ScorePercent = session.ScorePercent ?? 0,
            StartedAt = session.StartedAt,
            EndedAt = session.EndedAt,
            TimeTakenSeconds = session.EndedAt.HasValue
                ? (session.EndedAt.Value - session.StartedAt).TotalSeconds
                : 0,
            Status = session.Status
        };
    }

    public async Task<List<LeaderboardItemDto>> GetLeaderboardAsync(int technologyId)
    {
        var sessions = await _sessionRepo.GetTopSessionsByTechnologyAsync(technologyId, 20);

        return sessions.Select(s => new LeaderboardItemDto
        {
            UserName = s.User?.FullName ?? "Unknown",
            ScorePercent = s.ScorePercent ?? 0,
            CorrectAnswers = s.CorrectAnswers ?? 0,
            TotalQuestions = s.TotalQuestions,
            PlayedOn = s.CreatedOn
        }).ToList();
    }

    public async Task<bool> DeleteAttemptAsync(int sessionId)
    {
        return await _sessionRepo.DeleteSessionAsync(sessionId);
    }

    public async Task<int> GetAttemptCountAsync(int userId)
    {
        return await _sessionRepo.GetAttemptCountAsync(userId);
    }

    public async Task<AttemptListItemDto?> GetLatestAttemptAsync(int userId)
    {
        var s = await _sessionRepo.GetLatestSessionAsync(userId);
        if (s == null) return null;

        return new AttemptListItemDto
        {
            SessionId = s.Id,
            TechnologyName = s.Technology?.Name ?? "",
            DifficultyLevel = (int)(s.DifficultyLevel ?? 0),
            TotalQuestions = s.TotalQuestions,
            CorrectAnswers = s.CorrectAnswers ?? 0,
            ScorePercent = s.ScorePercent ?? 0,
            CreatedOn = s.CreatedOn,
            Status = s.Status
        };
    }

}
