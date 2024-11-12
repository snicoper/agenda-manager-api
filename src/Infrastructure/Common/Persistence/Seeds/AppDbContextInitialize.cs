﻿using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Infrastructure.Common.Persistence.Seeds.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds;

public class AppDbContextInitialize(
    AppDbContext context,
    IServiceProvider serviceProvider,
    UserManager userManager,
    RoleManager roleManager,
    PermissionManager permissionManager,
    IPasswordHasher passwordHasher,
    AuthorizationManager authorizationManager,
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
        _roles = await RoleSeed.InitializeAsync(context, roleManager);
        await PermissionSeed.InitializeAsync(context, permissionManager, authorizationManager, _roles);
        await UserSeed.InitializeAsync(context, userManager, passwordHasher, authorizationManager, _roles);
        await CalendarSeed.InitializeAsync(context, serviceProvider);
    }
}
