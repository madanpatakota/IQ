using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");

        builder.Property(q => q.QuestionText)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(q => q.OptionA).IsRequired().HasMaxLength(500);
        builder.Property(q => q.OptionB).IsRequired().HasMaxLength(500);
        builder.Property(q => q.OptionC).IsRequired().HasMaxLength(500);
        builder.Property(q => q.OptionD).IsRequired().HasMaxLength(500);

        builder.Property(q => q.CorrectOption)
            .IsRequired()
            .HasMaxLength(1);

        builder.HasOne(q => q.Technology)
            .WithMany(t => t.Questions)
            .HasForeignKey(q => q.TechnologyId);
    }
}
