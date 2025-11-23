using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Infrastructure.Persistence.Configurations;

public class QuizSessionAnswerConfiguration : IEntityTypeConfiguration<QuizSessionAnswer>
{
    public void Configure(EntityTypeBuilder<QuizSessionAnswer> builder)
    {
        builder.ToTable("QuizSessionAnswers");

        builder.Property(a => a.SelectedOption)
            .HasMaxLength(1);

        builder.HasOne(a => a.Session)
            .WithMany(s => s.Answers)
            .HasForeignKey(a => a.SessionId);

        builder.HasOne(a => a.Question)
            .WithMany()
            .HasForeignKey(a => a.QuestionId);
    }
}
