using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter()
        : base(
              v => v.ToUniversalTime(),                    // Convert to UTC before storing
              v => DateTime.SpecifyKind(v, DateTimeKind.Utc)) // Convert back as UTC
    { }
}

public class NullableUtcDateTimeConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableUtcDateTimeConverter()
        : base(
              v => v.HasValue ? v.Value.ToUniversalTime() : v,
              v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v)
    { }
}
