using Misard.IQs.Application.DTOs.Users;
using Misard.IQs.Application.Exceptions;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Application.Interfaces.Security;
using Misard.IQs.Application.Interfaces.Services;

namespace Misard.IQs.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtTokenGenerator _jwtGenerator;

    public AuthService(IUserRepository userRepo, IJwtTokenGenerator jwtGenerator)
    {
        _userRepo = userRepo;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<string> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email);
        if (user == null || string.IsNullOrEmpty(user.PasswordHash))
        {
            throw new BusinessException("Invalid email or password.");
        }

        var hashed = HashPassword(dto.Password);
        if (!string.Equals(user.PasswordHash, hashed, StringComparison.Ordinal))
        {
            throw new BusinessException("Invalid email or password.");
        }

        var token = _jwtGenerator.GenerateToken(user.Id, user.Email);
        return token;
    }

    private static string HashPassword(string password)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hashBytes = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}
