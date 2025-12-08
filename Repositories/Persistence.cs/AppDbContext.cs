using Microsoft.EntityFrameworkCore;
using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Technology> Technologies => Set<Technology>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<QuizSession> QuizSessions => Set<QuizSession>();
    public DbSet<QuizSessionAnswer> QuizSessionAnswers => Set<QuizSessionAnswer>();

    public DbSet<UserOtp> UserOtps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========== USERS ===========
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(200).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(500).IsRequired();
        });

        // =========== TECHNOLOGIES ===========
        modelBuilder.Entity<Technology>(entity =>
        {
            entity.ToTable("Technologies");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        });

        // =========== QUESTIONS ===========
        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Questions");
            entity.HasKey(e => e.Id);

            entity.HasOne(q => q.Technology)
                  .WithMany(t => t.Questions)
                  .HasForeignKey(q => q.TechnologyId);
        });

        // =========== QUIZ SESSIONS ===========
        modelBuilder.Entity<QuizSession>(entity =>
        {
            entity.ToTable("quizsessions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("userid");
            entity.Property(e => e.TechnologyId).HasColumnName("technologyid");
            entity.Property(e => e.DifficultyLevel).HasColumnName("difficultylevel");
            entity.Property(e => e.StartedAt).HasColumnName("startedat");
            entity.Property(e => e.EndedAt).HasColumnName("endedat");
            entity.Property(e => e.TotalQuestions).HasColumnName("totalquestions");
            entity.Property(e => e.CorrectAnswers).HasColumnName("correctanswers");

            // ⭐ FIX — specify correct decimal precision
            entity.Property(e => e.ScorePercent)
                  .HasColumnType("decimal(5,2)")
                  .HasColumnName("scorepercent");

            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedOn).HasColumnName("createdon");

            entity.HasOne(e => e.User)
                  .WithMany(u => u.QuizSessions)
                  .HasForeignKey(e => e.UserId);

            entity.HasOne(e => e.Technology)
                  .WithMany()
                  .HasForeignKey(e => e.TechnologyId);

            entity.HasMany(e => e.Answers)
                  .WithOne(a => a.Session)
                  .HasForeignKey(a => a.SessionId);
        });


        // =========== QUIZ SESSION ANSWERS ===========
        modelBuilder.Entity<QuizSessionAnswer>(entity =>
        {
            entity.ToTable("QuizSessionAnswers");
            entity.HasKey(e => e.Id);

            entity.HasOne(a => a.Session)
                  .WithMany(s => s.Answers)
                  .HasForeignKey(a => a.SessionId);

            entity.HasOne(a => a.Question)
                  .WithMany()
                  .HasForeignKey(a => a.QuestionId);
        });
    }
}
