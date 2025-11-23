using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Infrastructure.Persistence.Configurations;

public class QuizSessionConfiguration : IEntityTypeConfiguration<QuizSession>
{
    public void Configure(EntityTypeBuilder<QuizSession> builder)
    {
        builder.ToTable("QuizSessions");

        builder.Property(q => q.Status)
            .IsRequired();

        builder.HasOne(q => q.User)
            .WithMany(u => u.QuizSessions)
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(q => q.Technology)
            .WithMany()
            .HasForeignKey(q => q.TechnologyId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
