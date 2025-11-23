using Misard.IQs.Domain.Common;

namespace Misard.IQs.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    // For JWT login – optional but ready
    public string? PasswordHash { get; set; }

    public ICollection<QuizSession> QuizSessions { get; set; } = new List<QuizSession>();
}

