using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Resources.Services;
using AgendaManager.Domain.ResourceTypes.Services;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddUsersDomain();
        services.AddCalendarsDomain();
        services.AddResourceTypesDomain();
        services.AddResourcesDomain();

        return services;
    }

    private static void AddUsersDomain(this IServiceCollection services)
    {
        services.AddScoped<UserManager>();
        services.AddScoped<RoleManager>();
        services.AddScoped<PermissionManager>();
        services.AddScoped<IEmailUniquenessChecker, EmailUniquenessChecker>();
        services.AddScoped<AuthenticationService>();
        services.AddScoped<AuthorizationManager>();
    }

    private static void AddCalendarsDomain(this IServiceCollection services)
    {
        services.AddScoped<CalendarManager>();
    }

    private static void AddResourceTypesDomain(this IServiceCollection services)
    {
        services.AddScoped<ResourceTypeManager>();
    }

    private static void AddResourcesDomain(this IServiceCollection services)
    {
        services.AddScoped<ResourceManager>();
    }
}
