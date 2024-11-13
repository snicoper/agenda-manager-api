using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.ResourceTypes.Services;
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
        services.AddResourceTypes();

        return services;
    }

    private static void AddUsers(this IServiceCollection services)
    {
        services.AddScoped<UserManager>();
        services.AddScoped<RoleManager>();
        services.AddScoped<PermissionManager>();
        services.AddScoped<IEmailUniquenessChecker, EmailUniquenessChecker>();
        services.AddScoped<AuthenticationService>();
        services.AddScoped<AuthorizationManager>();
    }

    private static void AddCalendars(this IServiceCollection services)
    {
        services.AddScoped<CalendarManager>();
    }

    private static void AddResourceTypes(this IServiceCollection services)
    {
        services.AddScoped<ResourceTypeManager>();
    }
}
