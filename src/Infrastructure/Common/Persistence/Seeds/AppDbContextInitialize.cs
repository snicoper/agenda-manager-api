using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Persistence;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds;

public class AppDbContextInitialize(
    AppDbContext context,
    PasswordManager passwordManager,
    IAuthorizationManager authorizationManager,
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
        var createRoles = new List<Role>
        {
            Role.Create(RoleId.Create(), Roles.Admin),
            Role.Create(RoleId.Create(), Roles.Manager),
            Role.Create(RoleId.Create(), Roles.Client)
        };

        foreach (var role in createRoles.Where(role => context.Roles.All(r => r.Name != role.Name)))
        {
            context.Roles.Add(role);
        }

        await context.SaveChangesAsync();
    }

    private async Task CreateUsersAsync()
    {
        var passwordHash = passwordManager.HashPassword("Password4!");

        if (passwordHash.IsFailure || string.IsNullOrEmpty(passwordHash.Value))
        {
            throw new Exception("Failed to hash password.");
        }

        var roles = context.Roles.ToList();

        // Admin user.
        var admin = User.Create(
            UserId.Create(),
            EmailAddress.From("alice@example.com"),
            "alice",
            passwordHash.Value,
            "Alice",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(admin.Email)))
        {
            admin.ConfirmEmail();

            foreach (var role in roles)
            {
                await authorizationManager.AddRoleAsync(admin.Id, role.Id);
            }

            await context.Users.AddAsync(admin);
        }

        // Manager user.
        var manager = User.Create(
            UserId.Create(),
            EmailAddress.From("bob@example.com"),
            "bob",
            passwordHash.Value,
            "Bob",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(manager.Email)))
        {
            manager.ConfirmEmail();
            var rolesForManager = new List<Role>
            {
                roles.First(r => r.Name == Roles.Manager), roles.First(r => r.Name == Roles.Client)
            };

            foreach (var role in rolesForManager)
            {
                await authorizationManager.AddRoleAsync(manager.Id, role.Id);
            }

            await context.Users.AddAsync(manager);
        }

        // Client user.
        var client = User.Create(
            UserId.Create(),
            EmailAddress.From("carol@example.com"),
            "carol",
            passwordHash.Value,
            "Carol",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(client.Email)))
        {
            client.ConfirmEmail();
            var rolesForClient = new List<Role> { roles.First(r => r.Name == Roles.Client) };

            foreach (var role in rolesForClient)
            {
                await authorizationManager.AddRoleAsync(client.Id, role.Id);
            }

            await context.Users.AddAsync(client);
        }

        await context.SaveChangesAsync();
    }
}
