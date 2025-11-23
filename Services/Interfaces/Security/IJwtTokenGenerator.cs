namespace Misard.IQs.Application.Interfaces.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string email);
}
