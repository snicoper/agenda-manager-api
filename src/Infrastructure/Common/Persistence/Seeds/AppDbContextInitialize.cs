using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds;

public class AppDbContextInitialize(
    AppDbContext context,
    IAuthorizationManager authorizationManager,
    IPasswordHasher passwordHasher,
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
        await CreatePermissionsAsync();
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

    private async Task CreatePermissionsAsync()
    {
        var permissions = new List<Permission>
        {
            Permission.Create(PermissionId.Create(), Permissions.User.Read),
            Permission.Create(PermissionId.Create(), Permissions.User.Create),
            Permission.Create(PermissionId.Create(), Permissions.User.Update),
            Permission.Create(PermissionId.Create(), Permissions.User.Delete),
            Permission.Create(PermissionId.Create(), Permissions.Authorization.Read),
            Permission.Create(PermissionId.Create(), Permissions.Authorization.Create),
            Permission.Create(PermissionId.Create(), Permissions.Authorization.Update),
            Permission.Create(PermissionId.Create(), Permissions.Authorization.Delete)
        };

        foreach (var permission in permissions.Where(
                     permission => context.Permissions.All(p => p.Name != permission.Name)))
        {
            context.Permissions.Add(permission);
        }

        await context.SaveChangesAsync();
    }

    private async Task CreateUsersAsync()
    {
        var passwordHash = passwordHasher.HashPassword("Password4!");

        var roles = context.Roles.ToList();
        var permissions = context.Permissions.ToList();

        // Admin user.
        var admin = User.Create(
            UserId.Create(),
            EmailAddress.From("alice@example.com"),
            "alice",
            passwordHash,
            "Alice",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(admin.Email)))
        {
            admin.ConfirmEmail();

            await AddRolesToUser(admin, roles);
            await AddPermissionsToUser(admin, permissions);

            await context.Users.AddAsync(admin);
        }

        // Manager user.
        var manager = User.Create(
            UserId.Create(),
            EmailAddress.From("bob@example.com"),
            "bob",
            passwordHash,
            "Bob",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(manager.Email)))
        {
            manager.ConfirmEmail();
            var rolesForManager = new List<Role>
            {
                roles.First(r => r.Name == Roles.Manager), roles.First(r => r.Name == Roles.Client)
            };

            await AddRolesToUser(manager, rolesForManager);
            await AddPermissionsToUser(manager, permissions);

            await context.Users.AddAsync(manager);
        }

        // Client user.
        var client = User.Create(
            UserId.Create(),
            EmailAddress.From("carol@example.com"),
            "carol",
            passwordHash,
            "Carol",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(client.Email)))
        {
            client.ConfirmEmail();

            var rolesForClient = new List<Role> { roles.First(r => r.Name == Roles.Client) };
            await AddRolesToUser(client, rolesForClient);

            var permissionsForClient = new List<Permission>
            {
                permissions.First(p => p.Name == Permissions.User.Read),
                permissions.First(p => p.Name == Permissions.Authorization.Read)
            };

            await AddPermissionsToUser(client, permissionsForClient);

            await context.Users.AddAsync(client);
        }

        // Client user.
        var client2 = User.Create(
            UserId.Create(),
            EmailAddress.From("lexi@example.com"),
            "lexi",
            passwordHash,
            "Lexi",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(client2.Email)))
        {
            var rolesForClient = new List<Role> { roles.First(r => r.Name == Roles.Client) };
            await AddRolesToUser(client2, rolesForClient);

            var permissionsForClient = new List<Permission>
            {
                permissions.First(p => p.Name == Permissions.User.Read),
                permissions.First(p => p.Name == Permissions.Authorization.Read)
            };

            await AddPermissionsToUser(client2, permissionsForClient);
            client2.SetActiveState(false);

            await context.Users.AddAsync(client2);
        }

        await context.SaveChangesAsync();
    }

    private async Task AddRolesToUser(User user, List<Role> roles)
    {
        foreach (var role in roles)
        {
            await authorizationManager.AddRoleAsync(user.Id, role.Id);
        }
    }

    private async Task AddPermissionsToUser(User user, List<Permission> permissions)
    {
        foreach (var permission in permissions)
        {
            await authorizationManager.AddPermissionAsync(user.Id, permission.Id);
        }
    }
}
