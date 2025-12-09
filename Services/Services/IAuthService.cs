using Misard.IQs.Application.DTOs.Auth;

namespace Misard.IQs.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResultDto> LoginAsync(LoginRequestDto dto);

        Task<string> SendForgotPasswordOtpAsync(string phone);
        Task<bool> VerifyForgotPasswordOtpAsync(string sessionId, string otp);


        // Add these for forgot password
        Task SendOtpAsync(string email);
        Task<bool> VerifyOtpAsync(string email, string otp);

        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto);

    }
}
