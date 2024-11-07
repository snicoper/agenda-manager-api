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
    UserService userService,
    RoleService roleService,
    PermissionService permissionService,
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
        if (context.Roles.Any())
        {
            return;
        }

        var adminRole = await roleService.CreateAsync(RoleId.Create(), Roles.Admin);
        var managerRole = await roleService.CreateAsync(RoleId.Create(), Roles.Manager);
        var clientRole = await roleService.CreateAsync(RoleId.Create(), Roles.Client);

        _roles = [adminRole.Value!, managerRole.Value!, clientRole.Value!];

        foreach (var role in _roles.Where(role => context.Roles.All(r => r.Name != role.Name)))
        {
            context.Roles.Add(role);
        }

        await context.SaveChangesAsync();
    }

    private async Task CreatePermissionsAsync()
    {
        if (context.Permissions.Any())
        {
            return;
        }

        var userReadPermission = await permissionService.CreateAsync(PermissionId.Create(), Permissions.User.Read);
        var userCreatePermission = await permissionService.CreateAsync(PermissionId.Create(), Permissions.User.Create);
        var userUpdatePermission = await permissionService.CreateAsync(PermissionId.Create(), Permissions.User.Update);
        var userDeletePermission = await permissionService.CreateAsync(PermissionId.Create(), Permissions.User.Delete);

        _permissions =
        [
            userReadPermission.Value!,
            userCreatePermission.Value!,
            userUpdatePermission.Value!,
            userDeletePermission.Value!
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
        var adminResult = await userService.CreateAsync(
            userId: UserId.Create(),
            email: EmailAddress.From("alice@example.com"),
            passwordHash: passwordHash,
            firstName: "Alice",
            lastName: "Doe",
            active: true,
            emailConfirmed: true,
            cancellationToken: CancellationToken.None);

        if (adminResult.IsFailure || adminResult.Value is null)
        {
            return;
        }

        if (!await context.Users.AnyAsync(u => u.Email.Equals(adminResult.Value.Email)))
        {
            await context.SaveChangesAsync();

            var adminRole = _roles.First(r => r.Name == Roles.Admin);
            var managerRole = _roles.First(r => r.Name == Roles.Manager);
            var clientRole = _roles.First(r => r.Name == Roles.Client);

            await userAuthorizationManager.AddRoleToUserAsync(adminResult.Value.Id, adminRole.Id);
            await userAuthorizationManager.AddRoleToUserAsync(adminResult.Value.Id, managerRole.Id);
            await userAuthorizationManager.AddRoleToUserAsync(adminResult.Value.Id, clientRole.Id);
        }

        // Manager user.
        var managerResult = await userService.CreateAsync(
            UserId.Create(),
            EmailAddress.From("bob@example.com"),
            passwordHash,
            "Bob",
            "Doe",
            active: true,
            emailConfirmed: true,
            CancellationToken.None);

        if (managerResult.IsFailure || managerResult.Value is null)
        {
            return;
        }

        if (!await context.Users.AnyAsync(u => u.Email.Equals(managerResult.Value.Email)))
        {
            await context.SaveChangesAsync();

            var managerRole = _roles.First(r => r.Name == Roles.Manager);
            var clientRole = _roles.First(r => r.Name == Roles.Client);

            await userAuthorizationManager.AddRoleToUserAsync(managerResult.Value.Id, managerRole.Id);
            await userAuthorizationManager.AddRoleToUserAsync(managerResult.Value.Id, clientRole.Id);
        }

        // Client user.
        var clientResult = await userService.CreateAsync(
            UserId.Create(),
            EmailAddress.From("carol@example.com"),
            passwordHash,
            "Carol",
            "Doe",
            active: true,
            emailConfirmed: true,
            CancellationToken.None);

        if (clientResult.IsFailure || clientResult.Value is null)
        {
            return;
        }

        if (!await context.Users.AnyAsync(u => u.Email.Equals(clientResult.Value.Email)))
        {
            await context.SaveChangesAsync();

            var clientRole = _roles.First(r => r.Name == Roles.Client);
            await userAuthorizationManager.AddRoleToUserAsync(clientResult.Value.Id, clientRole.Id);
        }

        // Client user.
        var client2Result = await userService.CreateAsync(
            UserId.Create(),
            EmailAddress.From("lexi@example.com"),
            passwordHash,
            "Lexi",
            "Doe",
            active: true,
            emailConfirmed: false,
            CancellationToken.None);

        if (client2Result.IsFailure || client2Result.Value is null)
        {
            return;
        }

        if (!await context.Users.AnyAsync(u => u.Email.Equals(client2Result.Value.Email)))
        {
            var clientRole = _roles.First(r => r.Name == Roles.Client);
            await userAuthorizationManager.AddRoleToUserAsync(client2Result.Value.Id, clientRole.Id);
        }

        await context.SaveChangesAsync();
    }
}
