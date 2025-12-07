namespace Misard.IQs.Application.DTOs.Auth
{
    public class AuthResultDto
    {
        public string Token { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public int id { get;set; } = default!;
    }
}
