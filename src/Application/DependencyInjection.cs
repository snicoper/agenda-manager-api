using System.Reflection;
using AgendaManager.Application.Common.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddGlobalInjections(services);

        AddFluentValidation(services);

        AddMediator(services);

        return services;
    }

    private static void AddGlobalInjections(IServiceCollection services)
    {
    }

    private static void AddFluentValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private static void AddMediator(IServiceCollection services)
    {
        services.AddMediatR(
            configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                configuration.AddOpenBehavior(typeof(AuthorizationBehaviour<,>));
                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
                configuration.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                configuration.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
            });
    }
}
