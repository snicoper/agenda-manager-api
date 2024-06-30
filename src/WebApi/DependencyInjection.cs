using Asp.Versioning;

namespace AgendaManager.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        AddApiVersioning(services);

        return services;
    }

    private static void AddApiVersioning(IServiceCollection services)
    {
        IApiVersioningBuilder apiVersioningBuilder = services.AddApiVersioning(
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
}
