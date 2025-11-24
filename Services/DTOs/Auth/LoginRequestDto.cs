namespace Misard.IQs.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
