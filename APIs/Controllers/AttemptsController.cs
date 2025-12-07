using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Misard.IQs.Application.Interfaces.Services;

namespace Misard.IQs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttemptsController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public AttemptsController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        /// <summary>
        /// Get all quiz attempts for a given user
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserAttempts(int userId)
        {
            var result = await _quizService.GetAttemptsByUserAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Get detailed answers for one specific attempt session
        /// </summary>
        [HttpGet("details/{sessionId}")]
        public async Task<IActionResult> GetAttemptDetails(int sessionId)
        {
            var result = await _quizService.GetAttemptDetailsAsync(sessionId);
            return Ok(result);
        }
    }
}
