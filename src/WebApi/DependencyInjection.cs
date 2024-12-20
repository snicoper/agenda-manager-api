using System.Text.Json.Serialization;
using AgendaManager.Application.Common.Localization;
using AgendaManager.Application.Users.Services;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Options;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.Razor;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AgendaManager.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddHttpContextAccessor();

        AddGlobalInjections(services);
        AddCustomCors(services, configuration);

        services.AddControllersWithViews()
            .AddJsonOptions(
                options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; })
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(
                options =>
                {
                    // ReSharper disable once DelegateAnonymousParameter
                    // ReSharper disable once DelegateTypeParameter
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

        services.AddRouting(options => { options.LowercaseUrls = true; });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddExceptionHandler<CustomExceptionHandler>();

        AddApiVersioning(services);

        AddOpenTelemetry(services);

        AddRazorViewsForEmails(services);
        return services;
    }

    private static void AddGlobalInjections(IServiceCollection services)
    {
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
    }

    private static void AddApiVersioning(IServiceCollection services)
    {
        var apiVersioningBuilder = services.AddApiVersioning(
            options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

        apiVersioningBuilder.AddApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    private static void AddOpenTelemetry(IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("AgendaManager.WebApi"))
            .WithTracing(
                tracing =>
                {
                    tracing.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation();

                    tracing.AddOtlpExporter();
                });
    }

    private static void AddRazorViewsForEmails(IServiceCollection services)
    {
        services.Configure<RazorViewEngineOptions>(
            options =>
            {
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/Views/Emails/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
            });
    }

    private static void AddCustomCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var corsSettings = configuration.GetSection(CorsSettings.SectionName).Get<CorsSettings>();

        if (corsSettings is null)
        {
            throw new InvalidOperationException("Cors settings are not configured");
        }

        // Añadimos logging para debug
        var allowedDomains = corsSettings.AllowedDomains;
        foreach (var domain in allowedDomains)
        {
            Console.WriteLine($"Allowed domain: {domain}");
        }

        services.AddCors(
            options =>
            {
                options.AddPolicy(
                    corsSettings.DefaultPolicyName,
                    builder =>
                    {
                        builder
                            .WithOrigins(corsSettings.AllowedDomains.ToArray()) // Cambiamos a WithOrigins
                            .WithMethods(corsSettings.AllowedMethods)
                            .WithHeaders(corsSettings.AllowedHeaders)
                            .WithExposedHeaders(corsSettings.ExposedHeaders)
                            .AllowCredentials();
                    });
            });
    }
}
