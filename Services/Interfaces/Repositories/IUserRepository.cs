using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailorPhoneAsync(string email);
        Task<User?> GetByPhoneAsync(string phone);
        Task<User> AddAsync(User user);

        // 🔥 Required for Reset Password
        Task UpdateAsync(User user);
    }
}
