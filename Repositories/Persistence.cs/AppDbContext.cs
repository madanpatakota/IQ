using Microsoft.EntityFrameworkCore;
using Misard.IQs.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Misard.IQs.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Technology> Technologies => Set<Technology>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<QuizSession> QuizSessions => Set<QuizSession>();
    public DbSet<QuizSessionAnswer> QuizSessionAnswers => Set<QuizSessionAnswer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fix for PostgreSQL table name mismatch
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordSalt).HasColumnName("passwordsalt");
            entity.Property(e => e.PasswordHash).HasColumnName("passwordhash");
            entity.Property(e => e.FullName).HasColumnName("fullname");
            entity.Property(e => e.PhoneNumber).HasColumnName("phonenumber");
            entity.Property(e => e.CreatedOn).HasColumnName("createdon");
        });

        // modelBuilder.Entity<Technology>().ToTable("technologies");
        modelBuilder.Entity<Technology>(entity =>
        {
            entity.ToTable("technologies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
            entity.Property(e => e.CreatedOn).HasColumnName("createdon");

            // Relationships
            entity.HasMany(e => e.Questions)
                  .WithOne(q => q.Technology)
                  .HasForeignKey(q => q.TechnologyId);
        });

        //modelBuilder.Entity<Question>().ToTable("questions");
        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("questions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TechnologyId).HasColumnName("technologyid");
            entity.Property(e => e.DifficultyLevel).HasColumnName("difficultylevel");
            entity.Property(e => e.QuestionText).HasColumnName("questiontext");
            entity.Property(e => e.OptionA).HasColumnName("optiona");
            entity.Property(e => e.OptionB).HasColumnName("optionb");
            entity.Property(e => e.OptionC).HasColumnName("optionc");
            entity.Property(e => e.OptionD).HasColumnName("optiond");
            entity.Property(e => e.CorrectOption).HasColumnName("correctoption");
            entity.Property(e => e.Explanation).HasColumnName("explanation");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
            entity.Property(e => e.CreatedOn).HasColumnName("createdon");

            // FK Relationship
            entity.HasOne(q => q.Technology)
                  .WithMany(t => t.Questions)
                  .HasForeignKey(q => q.TechnologyId);
        });

        //modelBuilder.Entity<QuizSession>().ToTable("quiz_sessions");
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
            entity.Property(e => e.ScorePercent).HasColumnName("scorepercent");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedOn).HasColumnName("createdon");

            // Relationships
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

        //modelBuilder.Entity<QuizSessionAnswer>().ToTable("quiz_session_answers");
        modelBuilder.Entity<QuizSessionAnswer>(entity =>
        {
            entity.ToTable("quizsessionanswers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SessionId).HasColumnName("sessionid");
            entity.Property(e => e.QuestionId).HasColumnName("questionid");
            entity.Property(e => e.SelectedOption).HasColumnName("selectedoption");
            entity.Property(e => e.IsCorrect).HasColumnName("iscorrect");
            entity.Property(e => e.CreatedOn).HasColumnName("createdon");

            // Relationships
            entity.HasOne(e => e.Session)
                  .WithMany(s => s.Answers)
                  .HasForeignKey(e => e.SessionId);

            entity.HasOne(e => e.Question)
                  .WithMany()
                  .HasForeignKey(e => e.QuestionId);
        });


        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(new UtcDateTimeConverter());
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(new NullableUtcDateTimeConverter());
                }
            }
        }




        // Apply configuration classes
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Apply configuration classes automatically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
