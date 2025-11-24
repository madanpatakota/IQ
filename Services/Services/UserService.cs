using Misard.IQs.Application.DTOs.Users;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Application.Interfaces.Services;

namespace Misard.IQs.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : Map(user);
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null ? null : Map(user);
        }

        public async Task<UserDto?> GetByPhoneAsync(string phone)
        {
            var user = await _userRepository.GetByPhoneAsync(phone);
            return user == null ? null : Map(user);
        }

        private static UserDto Map(Misard.IQs.Domain.Entities.User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreatedOn = user.CreatedOn
            };
        }
    }
}
