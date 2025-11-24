using Microsoft.Extensions.DependencyInjection;
using Misard.IQs.Application.Interfaces.Services;
using Misard.IQs.Application.Services;

namespace Misard.IQs.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
