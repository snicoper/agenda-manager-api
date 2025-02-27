﻿using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.Policies;
using AgendaManager.Domain.Appointments.Services;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Policies;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.Policies;
using AgendaManager.Domain.ResourceManagement.Resources.Services;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;
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
        services.AddAuthorizationDomain();
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
        services.AddScoped<UserProfileManager>();

        services.AddTransient<IPasswordPolicy, PasswordPolicy>();
        services.AddTransient<AuthenticationService>();
    }

    private static void AddAuthorizationDomain(this IServiceCollection services)
    {
        services.AddScoped<RoleManager>();
        services.AddScoped<PermissionManager>();

        services.AddTransient<AuthorizationService>();
    }

    private static void AddCalendarsDomain(this IServiceCollection services)
    {
        services.AddScoped<CalendarManager>();
        services.AddScoped<CalendarSettingsManager>();
        services.AddScoped<CalendarHolidayManager>();

        services.AddTransient<ICalendarHolidayAvailabilityPolicy, CalendarHolidayAvailabilityPolicy>();
        services.AddTransient<
            ICalendarHolidayAvailabilityExcludeSelfPolicy,
            CalendarHolidayAvailabilityExcludeSelfPolicy>();
        services.AddTransient<ICalendarWeekDayAvailabilityPolicy, CalendarWeekDayAvailabilityPolicy>();
    }

    private static void AddResourceTypesDomain(this IServiceCollection services)
    {
        services.AddScoped<ResourceTypeManager>();

        services.AddTransient<ICanDeleteResourceTypePolicy, CanDeleteResourceTypePolicy>();
    }

    private static void AddResourcesDomain(this IServiceCollection services)
    {
        services.AddScoped<ResourceManager>();

        services.AddTransient<IHasResourcesInCalendarPolicy, HasResourcesInCalendarPolicy>();
        services.AddTransient<IResourceAvailabilityPolicy, ResourceAvailabilityPolicy>();
        services.AddTransient<ICanDeleteResourcePolicy, CanDeleteResourcePolicy>();
    }

    private static void AddServicesDomain(this IServiceCollection services)
    {
        services.AddScoped<ServiceManager>();

        services.AddTransient<IServiceRequirementsPolicy, ServiceRequirementsPolicy>();
        services.AddTransient<IHasServicesInCalendarPolicy, HasServicesInCalendarPolicy>();
    }

    private static void AddAppointmentsDomain(this IServiceCollection services)
    {
        services.AddScoped<AppointmentManager>();

        services.AddTransient<IAppointmentOverlapPolicy, AppointmentOverlapPolicy>();
        services.AddTransient<IAppointmentConfirmationStrategyPolicy, AppointmentConfirmationStrategyPolicy>();
        services.AddTransient<IAppointmentOverlapPolicy, AppointmentOverlapPolicy>();
        services.AddTransient<IHasAppointmentsInCalendarPolicy, HasAppointmentsInCalendarPolicy>();
    }
}
