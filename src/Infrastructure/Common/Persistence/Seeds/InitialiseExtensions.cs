using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds;

public static class InitialiseExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        AppDbContextInitialize initialise = scope.ServiceProvider.GetRequiredService<AppDbContextInitialize>();

        await initialise.InitialiseAsync();

        await initialise.SeedAsync();
    }
}
