﻿using System.Text.Json.Serialization;
using AgendaManager.Application.Common.Localization;
using AgendaManager.Application.Users.Services;
using AgendaManager.WebApi.Infrastructure;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.Razor;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AgendaManager.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddHttpContextAccessor();

        AddGlobalInjections(services);
        AddCors(services);

        services.AddControllers()
            .AddJsonOptions(
                options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; })
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(
                options =>
                {
                    // ReSharper disable once DelegateAnonymousParameter
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

        services.AddRouting(options => { options.LowercaseUrls = true; });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddExceptionHandler<CustomExceptionHandler>();

        AddApiVersioning(services);

        AddOpenTelemetry(services);

        // AddRazorViewsForEmails(services);
        return services;
    }

    private static void AddCors(IServiceCollection services)
    {
        services.AddCors(
            options =>
            {
                options.AddPolicy(
                    "DefaultCors",
                    builder =>
                    {
                        builder
                            .SetIsOriginAllowed(_ => true)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
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
}
