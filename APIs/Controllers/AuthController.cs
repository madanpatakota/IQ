using Microsoft.AspNetCore.Mvc;
using Misard.IQs.Application.DTOs.Users;
using Misard.IQs.Application.Interfaces.Services;

namespace Misard.IQs.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var userId = await _userService.RegisterAsync(dto);
        return Ok(new
        {
            userId,
            message = "Registration successful. You can now login to view detailed results and history."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        return Ok(new
        {
            token,
            message = "Login successful."
        });
    }
}
