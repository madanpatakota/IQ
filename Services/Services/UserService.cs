using Misard.IQs.Application.DTOs.Users;
using Misard.IQs.Application.Exceptions;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Application.Interfaces.Services;
using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<int> RegisterAsync(RegisterUserDto dto)
    {
        var existing = await _userRepo.GetByEmailAsync(dto.Email);
        if (existing != null)
        {
            throw new BusinessException("Email is already registered.");
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            // For now, simple: store null or hashed later
            PasswordHash = dto.Password != null
                ? HashPassword(dto.Password)
                : null
        };

        user = await _userRepo.AddAsync(user);
        return user.Id;
    }

    private static string HashPassword(string password)
    {
        // Simple SHA256 hash – for demo.
        // In production, use a proper password hashing library (e.g., BCrypt).
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hashBytes = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}
