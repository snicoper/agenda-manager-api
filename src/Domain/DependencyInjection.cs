using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.Policies;
using AgendaManager.Domain.Appointments.Services;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Policies;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Resources.Policies;
using AgendaManager.Domain.Resources.Services;
using AgendaManager.Domain.ResourceTypes.Services;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.Policies;
using AgendaManager.Domain.Services.Services;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Policies;
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
        services.AddServicesDomain();
        services.AddAppointmentsDomain();

        return services;
    }

    private static void AddUsersDomain(this IServiceCollection services)
    {
        services.AddScoped<UserManager>();
        services.AddScoped<RoleManager>();
        services.AddScoped<PermissionManager>();

        services.AddTransient<IEmailUniquenessPolicy, EmailUniquenessPolicy>();
        services.AddTransient<AuthenticationService>();
        services.AddTransient<AuthorizationService>();
    }

    private static void AddCalendarsDomain(this IServiceCollection services)
    {
        services.AddScoped<CalendarManager>();

        services.AddTransient<ICalendarHolidayAvailabilityPolicy, CalendarHolidayAvailabilityPolicy>();
    }

    private static void AddResourceTypesDomain(this IServiceCollection services)
    {
        services.AddScoped<ResourceTypeManager>();

        services.AddTransient<IResourceAvailabilityPolicy, ResourceAvailabilityPolicy>();
    }

    private static void AddResourcesDomain(this IServiceCollection services)
    {
        services.AddScoped<ResourceManager>();
    }

    private static void AddServicesDomain(this IServiceCollection services)
    {
        services.AddScoped<ServiceManager>();

        services.AddTransient<IServiceRequirementsPolicy, ServiceRequirementsPolicy>();
    }

    private static void AddAppointmentsDomain(this IServiceCollection services)
    {
        services.AddScoped<AppointmentManager>();

        services.AddTransient<IAppointmentOverlapPolicy, AppointmentOverlapPolicy>();
        services.AddTransient<IAppointmentCreationStrategyPolicy, AppointmentCreationStrategyPolicy>();
        services.AddTransient<IAppointmentOverlapPolicy, AppointmentOverlapPolicy>();
    }
}
