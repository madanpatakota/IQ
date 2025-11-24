namespace Misard.IQs.Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
