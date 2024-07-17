using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddUsers();

        return services;
    }

    private static void AddUsers(this IServiceCollection services)
    {
        services.AddScoped<IPasswordPolicy, StrongPasswordPolicy>();
        services.AddScoped<UserPasswordService>();
        services.AddScoped<IUserEmailPolicy, StandardUserEmailPolicy>();
        services.AddScoped<UserEmailService>();
        services.AddScoped<UserAuthenticationService>();
    }
}
