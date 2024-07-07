using Microsoft.AspNetCore.Builder;

namespace AgendaManager.Infrastructure.Common.Middlewares;

public static class InfrastructureBuilderExtensions
{
    public static void UseInfrastructureMiddleware(this WebApplication app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
    }
}
