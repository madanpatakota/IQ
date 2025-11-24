using System.Security.Cryptography;
using System.Text;
using Misard.IQs.Application.DTOs.Auth;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Application.Interfaces.Security;
using Misard.IQs.Application.Interfaces.Services;
using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        // ----------------------------------------------------------
        // REGISTER
        // ----------------------------------------------------------
        public async Task<AuthResultDto> RegisterAsync(RegisterRequestDto dto)
        {
            var existingEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                throw new Exception("Email already registered.");

            var existingPhone = await _userRepository.GetByPhoneAsync(dto.PhoneNumber);
            if (existingPhone != null)
                throw new Exception("Phone number already registered.");

            CreatePasswordHash(dto.Password, out var hash, out var salt);

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = hash,
                PasswordSalt = salt,
                CreatedOn = DateTime.UtcNow
            };

            user = await _userRepository.AddAsync(user);

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);

            return new AuthResultDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        // ----------------------------------------------------------
        // LOGIN
        // ----------------------------------------------------------
        public async Task<AuthResultDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByPhoneAsync(dto.PhoneNumber);
            if (user == null)
                throw new Exception("Invalid phone number or password.");

            if (!VerifyPassword(dto.Password, user.PasswordHash!, user.PasswordSalt!))
                throw new Exception("Invalid phone number or password.");

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);

            return new AuthResultDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        // ----------------------------------------------------------
        // PASSWORD UTILS
        // ----------------------------------------------------------
        private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA256();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA256(storedSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(storedHash);
        }
    }
}
