using System.Text;
using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Settings;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Infrastructure.Authentication;
using AgendaManager.Infrastructure.Common.Clock;
using AgendaManager.Infrastructure.Common.Persistence;
using AgendaManager.Infrastructure.Common.Persistence.Interceptors;
using AgendaManager.Infrastructure.Common.Persistence.Seeds;
using AgendaManager.Infrastructure.Users;
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

        AddGlobalDiServices(services);

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

        services.AddOptions<EmailSenderSettings>()
            .Bind(configuration.GetSection(EmailSenderSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    private static void AddGlobalDiServices(IServiceCollection services)
    {
        // Common.
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        // Repositories.
        services.AddScoped<IUsersRepository, UserRepository>();
    }

    private static void AddDatabase(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

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
