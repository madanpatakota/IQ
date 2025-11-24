using Microsoft.AspNetCore.Mvc;
using Misard.IQs.Application.DTOs.Auth;
using Misard.IQs.Application.Interfaces.Services;

namespace Misard.IQs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            var result = await _auth.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await _auth.LoginAsync(dto);
            return Ok(result);
        }
    }

}
