using Misard.IQs.Application.DTOs.Auth;

namespace Misard.IQs.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResultDto> LoginAsync(LoginRequestDto dto);
    }
}
