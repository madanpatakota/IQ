using Misard.IQs.Domain.Common;

namespace Misard.IQs.Domain.Entities;

public class Technology : BaseEntity
{
    public string Name { get; set; } = null!;          // e.g. "JavaScript"
    public string Category { get; set; } = null!;      // "Frontend", "Middleware", "Backend", "Cloud"
    public bool IsActive { get; set; } = true;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}

