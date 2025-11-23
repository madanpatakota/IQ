using Misard.IQs.Application.DTOs.Users;

namespace Misard.IQs.Application.Interfaces.Services;

public interface IUserService
{
    Task<int> RegisterAsync(RegisterUserDto dto);
}
