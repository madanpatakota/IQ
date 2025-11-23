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

        // Apply configuration classes automatically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
