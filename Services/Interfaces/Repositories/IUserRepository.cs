using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    //
    //Task<User> CreateAsync(User user);
    Task<User> AddAsync(User user);   // <-- ADD THIS
}
