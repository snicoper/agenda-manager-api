using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds;

public static class InitialiseExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        if (!app.Environment.IsProduction())
        {
            await InitialiseForNonProduction(app);
        }
    }

    private static async Task InitialiseForNonProduction(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialise = scope.ServiceProvider.GetRequiredService<AppDbContextInitialize>();

        await initialise.InitialiseAsync();

        await initialise.SeedAsync();
    }
}
