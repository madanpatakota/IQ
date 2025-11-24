namespace Misard.IQs.Application.DTOs.Users;

public class RegisterUserDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Password { get; set; } // optional
}
