using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds;

public class AppDbContextInitialize(
    AppDbContext context,
    IPasswordHasher passwordHasher,
    UserAuthorizationManager userAuthorizationManager,
    ILogger<AppDbContextInitialize> logger)
{
    private static List<Role> _roles = [];
    private static List<Permission> _permissions = [];

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
        _roles =
        [
            Role.Create(RoleId.Create(), Roles.Admin),
            Role.Create(RoleId.Create(), Roles.Manager),
            Role.Create(RoleId.Create(), Roles.Client)
        ];

        foreach (var role in _roles.Where(role => context.Roles.All(r => r.Name != role.Name)))
        {
            context.Roles.Add(role);
        }

        await context.SaveChangesAsync();
    }

    private async Task CreatePermissionsAsync()
    {
        _permissions =
        [
            Permission.Create(PermissionId.Create(), Permissions.User.Read),
            Permission.Create(PermissionId.Create(), Permissions.User.Create),
            Permission.Create(PermissionId.Create(), Permissions.User.Update),
            Permission.Create(PermissionId.Create(), Permissions.User.Delete)
        ];

        foreach (var permission in _permissions.Where(
                     permission => context.Permissions.All(p => p.Name != permission.Name)))
        {
            context.Permissions.Add(permission);
        }

        await context.SaveChangesAsync();

        // Asignar todos los permisos a role Admin.
        var adminRole = _roles.First(r => r.Name == Roles.Admin);
        foreach (var permission in _permissions)
        {
            await userAuthorizationManager.AddPermissionToRole(adminRole.Id, permission.Id);
        }

        // Asignar todos permisos a role Manager.
        var managerRole = _roles.First(r => r.Name == Roles.Manager);
        foreach (var permission in _permissions)
        {
            await userAuthorizationManager.AddPermissionToRole(managerRole.Id, permission.Id);
        }

        // Asignar solo permisos de lectura a role Client.
        var clientRole = _roles.First(r => r.Name == Roles.Client);
        foreach (var permission in _permissions.Where(p => p.Name.Contains("read")))
        {
            await userAuthorizationManager.AddPermissionToRole(clientRole.Id, permission.Id);
        }

        await context.SaveChangesAsync();
    }

    private async Task CreateUsersAsync()
    {
        var passwordHash = passwordHasher.HashPassword("Password4!");

        // Admin user.
        var admin = User.Create(
            UserId.Create(),
            EmailAddress.From("alice@example.com"),
            passwordHash,
            "Alice",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(admin.Email)))
        {
            admin.ConfirmEmail();

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();

            var adminRole = _roles.First(r => r.Name == Roles.Admin);
            var managerRole = _roles.First(r => r.Name == Roles.Manager);
            var clientRole = _roles.First(r => r.Name == Roles.Client);

            await userAuthorizationManager.AddRoleToUserAsync(admin.Id, adminRole.Id);
            await userAuthorizationManager.AddRoleToUserAsync(admin.Id, managerRole.Id);
            await userAuthorizationManager.AddRoleToUserAsync(admin.Id, clientRole.Id);
        }

        // Manager user.
        var manager = User.Create(
            UserId.Create(),
            EmailAddress.From("bob@example.com"),
            passwordHash,
            "Bob",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(manager.Email)))
        {
            manager.ConfirmEmail();

            await context.Users.AddAsync(manager);
            await context.SaveChangesAsync();

            var managerRole = _roles.First(r => r.Name == Roles.Manager);
            var clientRole = _roles.First(r => r.Name == Roles.Client);

            await userAuthorizationManager.AddRoleToUserAsync(manager.Id, managerRole.Id);
            await userAuthorizationManager.AddRoleToUserAsync(manager.Id, clientRole.Id);
        }

        // Client user.
        var client = User.Create(
            UserId.Create(),
            EmailAddress.From("carol@example.com"),
            passwordHash,
            "Carol",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(client.Email)))
        {
            client.ConfirmEmail();

            await context.Users.AddAsync(client);
            await context.SaveChangesAsync();

            var clientRole = _roles.First(r => r.Name == Roles.Client);
            await userAuthorizationManager.AddRoleToUserAsync(client.Id, clientRole.Id);
        }

        // Client user.
        var client2 = User.Create(
            UserId.Create(),
            EmailAddress.From("lexi@example.com"),
            passwordHash,
            "Lexi",
            "Doe");

        if (!await context.Users.AnyAsync(u => u.Email.Equals(client2.Email)))
        {
            client2.SetActiveState(false);

            await context.Users.AddAsync(client2);
            await context.SaveChangesAsync();

            var clientRole = _roles.First(r => r.Name == Roles.Client);
            await userAuthorizationManager.AddRoleToUserAsync(client2.Id, clientRole.Id);
        }

        await context.SaveChangesAsync();
    }
}
