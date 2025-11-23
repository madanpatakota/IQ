using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Misard.IQs.Application.DTOs.Quiz;
using Misard.IQs.Application.Interfaces.Services;

namespace Misard.IQs.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    /// <summary>
    /// Start a new quiz session (15 minutes).
    /// </summary>
    [HttpPost("start")]
    [AllowAnonymous]
    public async Task<IActionResult> StartQuiz([FromBody] StartQuizRequestDto dto)
    {
        var result = await _quizService.StartQuizAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Submit answers for a quiz session.
    /// </summary>
    [HttpPost("submit")]
    [AllowAnonymous] // student can attempt without login
    public async Task<IActionResult> SubmitQuiz([FromBody] SubmitQuizRequestDto dto)
    {
        var result = await _quizService.SubmitQuizAsync(dto);

        // Here we have result (with marks), but according to your rule:
        // "If student wants to see marks then he should register"
        // Frontend can call this endpoint just to complete test,
        // and then call GetResult below only AFTER login.

        return Ok(new
        {
            sessionId = result.SessionId,
            message = "Quiz submitted successfully. Please register/login to view your detailed marks."
        });
    }

    /// <summary>
    /// Get detailed quiz result. Require login.
    /// </summary>
    [HttpGet("{sessionId:int}/result")]
    [Authorize]  // must be logged in (JWT)
    public async Task<IActionResult> GetResult(int sessionId)
    {
        var result = await _quizService.GetResultAsync(sessionId);
        return Ok(result);
    }
}
