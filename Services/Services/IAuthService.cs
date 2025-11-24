using Misard.IQs.Application.DTOs.Auth;

namespace Misard.IQs.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResultDto> LoginAsync(LoginRequestDto request);
    }
}
