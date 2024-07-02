using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds;

public class AppDbContextInitialize(
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ILogger<AppDbContextInitialize> logger)
{
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
        await CreateRolesAsync();
        await CreateUsersAsync();
    }

    private async Task CreateRolesAsync()
    {
        var createRole = new List<IdentityRole<Guid>> { new(Roles.Admin), new(Roles.Staff) };

        foreach (var identityRole in createRole.Where(identityRole => roleManager.Roles.All(r => r.Name != identityRole.Name)))
        {
            await roleManager.CreateAsync(identityRole);
        }
    }

    private async Task CreateUsersAsync()
    {
        const string password = "Password4!";

        // Admin user.
        var user = new User { UserName = "alice@example.com", Email = "alice@example.com", EmailConfirmed = true };

        if (!await userManager.Users.AnyAsync(u => u.Email == user.Email))
        {
            await userManager.CreateAsync(user, password);
            var rolesToAdd = new[] { Roles.Admin, Roles.Staff };
            await userManager.AddToRolesAsync(user, rolesToAdd);
        }

        // Staff user.
        user = new User { UserName = "bob@example.com", Email = "bob@example.com", EmailConfirmed = true };

        if (!await userManager.Users.AnyAsync(u => u.Email == user.Email))
        {
            await userManager.CreateAsync(user, password);
            var rolesToAdd = new[] { Roles.Staff };
            await userManager.AddToRolesAsync(user, rolesToAdd);
        }

        await context.SaveChangesAsync(CancellationToken.None);
    }
}
