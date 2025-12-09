using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Misard.IQs.Application.DTOs.Auth;
using Misard.IQs.Application.Interfaces.Services;
using Misard.IQs.Application.Services;
using Misard.IQs.Domain.Entities;
using Misard.IQs.Infrastructure.Services;

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


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            var sessionId = await _auth.SendForgotPasswordOtpAsync(dto.Phone);
            return Ok(new { sessionId });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto dto)
        {
            var result = await _auth.VerifyForgotPasswordOtpAsync(dto.SessionId, dto.Otp);

            if (result)
                return Ok(new { message = "OTP Verified" });

            return BadRequest(new { message = "Invalid OTP" });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            var result = await _auth.ResetPasswordAsync(dto);

            if (!result)
                return BadRequest(new { message = "Failed to reset password" });

            return Ok(new { message = "Password reset successful" });
        }



        //Needs to reset the Password API.





        //[HttpPost("forgot-password/send-otp")]
        //public async Task<IActionResult> SendOtp([FromBody] string email)
        //{
        //    await _auth.SendOtpAsync(email);
        //    return Ok(new { message = "OTP sent to email." });
        //}



        //[HttpPost("forgot-password/verify-otp")]
        //public async Task<IActionResult> VerifyOtp(VerifyOtpRequestDto dto)
        //{
        //    var ok = await _auth.VerifyOtpAsync(dto.Email, dto.Otp);

        //    if (!ok)
        //        return BadRequest(new { message = "Invalid or expired OTP" });

        //    return Ok(new { message = "OTP verified" });
        //}




    }

}
