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

        public async Task<AuthResultDto> RegisterAsync(RegisterRequestDto request)
        {
            // check if email already exists
            var existing = await _userRepository.GetByEmailAsync(request.Email);
            if (existing != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CreatedOn = DateTime.UtcNow
            };

            user = await _userRepository.AddAsync(user);

            // NOTE: your IJwtTokenGenerator signature: GenerateToken(int userId, string email)
            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);

            return new AuthResultDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<AuthResultDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                // you can either:
                // 1) throw error
                // 2) auto-register (here we throw)
                throw new InvalidOperationException("User not found. Please register first.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);

            return new AuthResultDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
