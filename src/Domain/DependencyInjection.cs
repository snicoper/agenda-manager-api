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
        services.AddScoped<UserManager>();
        services.AddScoped<RoleManager>();
        services.AddScoped<PermissionManager>();
        services.AddScoped<IUserPasswordManager, UserUserPasswordManager>();
        services.AddScoped<IUserEmailManager, UserEmailManager>();
        services.AddScoped<UserEmailManager>();
        services.AddScoped<UserAuthenticationService>();
        services.AddScoped<UserAuthorizationManager>();
    }

    private static void AddCalendars(this IServiceCollection services)
    {
        services.AddScoped<CalendarManager>();
    }
}
