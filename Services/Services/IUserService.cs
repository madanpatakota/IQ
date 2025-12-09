using Misard.IQs.Application.DTOs.Users;

namespace Misard.IQs.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> GetByEmailorPhoneAsync(string email);
        Task<UserDto?> GetByPhoneAsync(string phone);
    }
}
