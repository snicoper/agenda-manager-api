using System.Text;
using AgendaManager.Application.Authentication.Interfaces;
using AgendaManager.Application.Authorization.Interfaces;
using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Application.Users.Interfaces;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.AuditRecords.Interfaces;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.ResourceTypes.Interfaces;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Infrastructure.Appointments.Persistence.Repositories;
using AgendaManager.Infrastructure.AuditRecords.Persistence.Repositories;
using AgendaManager.Infrastructure.Authorization.Persistence.Repositories;
using AgendaManager.Infrastructure.Authorization.Services;
using AgendaManager.Infrastructure.Calendars.Persistence.Repositories;
using AgendaManager.Infrastructure.Common.Clock;
using AgendaManager.Infrastructure.Common.Emails;
using AgendaManager.Infrastructure.Common.Emails.Interfaces;
using AgendaManager.Infrastructure.Common.Emails.Options;
using AgendaManager.Infrastructure.Common.Options;
using AgendaManager.Infrastructure.Common.Persistence;
using AgendaManager.Infrastructure.Common.Persistence.Interceptors;
using AgendaManager.Infrastructure.Common.Persistence.Seeds;
using AgendaManager.Infrastructure.Resources.Persistence.Repositories;
using AgendaManager.Infrastructure.ResourceTypes.Repositories;
using AgendaManager.Infrastructure.Services.Persistence.Repositories;
using AgendaManager.Infrastructure.Users.Authentication;
using AgendaManager.Infrastructure.Users.Emails.ConfirmEmailResent;
using AgendaManager.Infrastructure.Users.Emails.SendRecoveryPassword;
using AgendaManager.Infrastructure.Users.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgendaManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        ValidateOptionsOnStartUp(services, configuration);

        AddGlobalInjections(services);

        AddDatabase(services, configuration, environment);

        AddAuthentication(services, configuration);

        return services;
    }

    private static void ValidateOptionsOnStartUp(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<EmailSettings>()
            .Bind(configuration.GetSection(EmailSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ClientAppSettings>()
            .Bind(configuration.GetSection(ClientAppSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ClientApiSettings>()
            .Bind(configuration.GetSection(ClientApiSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    private static void AddGlobalInjections(IServiceCollection services)
    {
        // Common.
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

        // AuditRecords.
        services.AddScoped<IAuditRecordRepository, AuditRecordRepository>();

        // Authorization.
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IAuthorizationCheckService, AuthorizationCheckService>();

        // Users.
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddTransient<ISendRecoveryPasswordService, SendRecoveryPasswordService>();
        services.AddTransient<ISendConfirmEmailResentService, SendConfirmEmailResentService>();

        // Calendars.
        services.AddScoped<ICalendarRepository, CalendarRepository>();
        services.AddScoped<ICalendarHolidayRepository, CalendarHolidayRepository>();
        services.AddScoped<ICalendarConfigurationRepository, CalendarConfigurationRepository>();

        // ResourceTypes.
        services.AddScoped<IResourceTypeRepository, ResourceTypeRepository>();

        // Resources.
        services.AddScoped<IResourceRepository, ResourceRepository>();

        // Services.
        services.AddScoped<IServiceRepository, ServiceRepository>();

        // Appointments.
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
    }

    private static void AddDatabase(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddDbContext<AppDbContext>(
            (provider, options) =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                       throw new NullReferenceException("No connection string found in configuration");

                options.AddInterceptors(provider.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(connectionString);

                if (!environment.IsProduction())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddScoped<AppDbContextInitialize>();
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthorizationChecker, AuthorizationChecker>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

        JwtSettings jwtSettings = new();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
    }
}
