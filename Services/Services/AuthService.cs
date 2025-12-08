using Misard.IQs.Application.DTOs.Auth;
using Misard.IQs.Application.Exceptions;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Application.Interfaces.Security;
using Misard.IQs.Application.Interfaces.Services;
using Misard.IQs.Application.Services;
using Misard.IQs.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Misard.IQs.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IOtpRepository _otpRepo;
        private readonly EmailService _email;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator, 
            IOtpRepository otpRepo,
           EmailService emailService)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _otpRepo = otpRepo;
            _email = emailService;
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
                PhoneNumber = user.PhoneNumber,
                id = user.Id
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



        private readonly string _twoFactorApiKey = "48c4fdb5-d422-11f0-a6b2-0200cd936042";

        public async Task<string> SendForgotPasswordOtpAsync(string phone)
        {
            string url = $"https://2factor.in/API/V1/{_twoFactorApiKey}/SMS/{phone}/AUTOGEN";

            using var client = new HttpClient();
            try
            {
                var response = await client.GetStringAsync(url);
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                if (result.Status == "Success")
                return result.Details;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            throw new BusinessException("Failed to send OTP.");
        }

        public async Task<bool> VerifyForgotPasswordOtpAsync(string sessionId, string otp)
        {
            string url = $"https://2factor.in/API/V1/{_twoFactorApiKey}/SMS/VERIFY/{sessionId}/{otp}";

            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);

            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

            return result.Status == "Success";
        }



        //send otp
        public async Task SendOtpAsync(string email)
        {
            var otp = new Random().Next(1000, 9999).ToString();

            await _otpRepo.SaveOtpAsync(new UserOtp
            {
                Email = email,
                Otp = otp,
                Expiry = DateTime.UtcNow.AddMinutes(5)
            });

            await _email.SendEmailAsync(
                email,
                "Misard IQs - Password Reset OTP",
                $"<h2>Your OTP is <b>{otp}</b></h2><p>Valid for 5 minutes.</p>"
            );
        }


        //verify otp
        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var record = await _otpRepo.GetLatestOtpAsync(email);

            if (record == null)
                return false;

            if (record.Expiry < DateTime.UtcNow)
                return false;

            return record.Otp == otp;
        }



    }
}
