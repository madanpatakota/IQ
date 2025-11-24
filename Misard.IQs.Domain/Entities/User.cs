using Misard.IQs.Domain.Common;

namespace Misard.IQs.Domain.Entities;

public class User : BaseEntity
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public DateTime CreatedOn { get; set; }

    // NEW for auth
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }

    public ICollection<QuizSession> QuizSessions { get; set; } = new List<QuizSession>();
}

