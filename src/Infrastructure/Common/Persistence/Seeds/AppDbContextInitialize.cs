using AgendaManager.Domain.Authorization;
using AgendaManager.Infrastructure.Common.Persistence.Seeds.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds;

public class AppDbContextInitialize(
    AppDbContext context,
    IServiceProvider serviceProvider,
    ILogger<AppDbContextInitialize> logger)
{
    private static List<Role> _roles = [];

    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        var rolesResult = await RoleSeed.InitializeAsync(context, serviceProvider);

        // Asegurarse de que los roles se hayan inicializado correctamente en segundas iteraciones.
        _roles = rolesResult.Count > 0 ? rolesResult : await context.Roles.ToListAsync();

        await PermissionSeed.InitializeAsync(context, serviceProvider, _roles);
        await UserSeed.InitializeAsync(context, serviceProvider, _roles);
        await CalendarSeed.InitializeAsync(context, serviceProvider);
        await ResourceTypeSeed.InitializeAsync(context, _roles, serviceProvider);
        await ResourceSeed.InitializeAsync(context, serviceProvider);
        await ResourceScheduleSeed.InitializeAsync(context);
        await ServiceSeed.InitializeAsync(context, serviceProvider);
    }
}
