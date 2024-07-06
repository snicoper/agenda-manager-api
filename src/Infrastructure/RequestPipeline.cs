using AgendaManager.Infrastructure.Common.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace AgendaManager.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder UseInfrastructureMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<EventualConsistencyMiddleware>();

        return builder;
    }
}
