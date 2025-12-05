using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Application.Interfaces.Security;
using Misard.IQs.Infrastructure.Persistence;
using Misard.IQs.Infrastructure.Repositories;
using Misard.IQs.Infrastructure.Security;
using Misard.IQs.Application.Interfaces.Services;
using Misard.IQs.Infrastructure.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL;
//using Microsoft.EntityFrameworkCore;


namespace Misard.IQs.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString))
            ;

        // Repositories
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<ITechnologyRepository, TechnologyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IQuizSessionRepository, QuizSessionRepository>();

        // Security
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        // Services
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
