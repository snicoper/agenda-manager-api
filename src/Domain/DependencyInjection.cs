using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddUsers();
        services.AddCalendars();

        return services;
    }

    private static void AddUsers(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<RoleService>();
        services.AddScoped<PermissionService>();
        services.AddScoped<IPasswordPolicy, StrongPasswordPolicy>();
        services.AddScoped<UserPasswordService>();
        services.AddScoped<IUserEmailPolicy, StandardUserEmailPolicy>();
        services.AddScoped<UserEmailService>();
        services.AddScoped<UserAuthenticationService>();
        services.AddScoped<UserAuthorizationManager>();
    }

    private static void AddCalendars(this IServiceCollection services)
    {
        services.AddScoped<CalendarService>();
    }
}
