using System.Text;
using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Infrastructure.Common.Clock;
using AgendaManager.Infrastructure.Common.Persistence;
using AgendaManager.Infrastructure.Common.Persistence.Interceptors;
using AgendaManager.Infrastructure.Common.Persistence.Seeds;
using AgendaManager.Infrastructure.Common.Services.Emails;
using AgendaManager.Infrastructure.Users;
using AgendaManager.Infrastructure.Users.Repository;
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
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<EmailSenderSettings>()
            .Bind(configuration.GetSection(EmailSenderSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    private static void AddGlobalInjections(IServiceCollection services)
    {
        // Common.
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        // Repositories.
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
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

        JwtOptions jwtOptions = new();
        configuration.Bind(JwtOptions.SectionName, jwtOptions);

        services.AddSingleton(Options.Create(jwtOptions));

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
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
    }
}
