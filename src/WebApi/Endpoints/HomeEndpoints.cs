namespace AgendaManager.WebApi.Endpoints;

public static class HomeEndpoints
{
    public static IEndpointRouteBuilder MapHomeEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(string.Empty, Get);

        return builder;
    }

    private static IResult Get()
    {
        return Results.Ok("Hello World!");
    }
}
