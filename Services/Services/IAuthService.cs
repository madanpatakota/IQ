using Misard.IQs.Application.DTOs.Users;

namespace Misard.IQs.Application.Interfaces.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginRequestDto dto);  // returns JWT token
}
