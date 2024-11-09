using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds;

public class AppDbContextInitialize(
    AppDbContext context,
    UserManager userManager,
    RoleManager roleManager,
    PermissionManager permissionManager,
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

        var adminRole = await roleManager.CreateRoleAsync(RoleId.Create(), Roles.Admin, "Admin role");
        var managerRole = await roleManager.CreateRoleAsync(RoleId.Create(), Roles.Manager, "Manager role");
        var clientRole = await roleManager.CreateRoleAsync(RoleId.Create(), Roles.Client, "Client role");

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

        // Appointment permissions.
        var appointmentReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Appointments.Read);
        var appointmentCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Appointments.Create);
        var appointmentUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Appointments.Update);
        var appointmentDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Appointments.Delete);

        // AppointmentStatus permissions.
        var appointmentStatusReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.AppointmentStatuses.Read);
        var appointmentStatusCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.AppointmentStatuses.Create);
        var appointmentStatusUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.AppointmentStatuses.Update);
        var appointmentStatusDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.AppointmentStatuses.Delete);

        // Calendar permissions.
        var calendarReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Calendars.Read);
        var calendarCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Calendars.Create);
        var calendarUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Calendars.Update);
        var calendarDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Calendars.Delete);

        // CalendarHoliday permissions.
        var calendarHolidayReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.CalendarHolidays.Read);
        var calendarHolidayCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.CalendarHolidays.Create);
        var calendarHolidayUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.CalendarHolidays.Update);
        var calendarHolidayDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.CalendarHolidays.Delete);

        // Resource permissions.
        var resourceReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Resources.Read);
        var resourceCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Resources.Create);
        var resourceUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Resources.Update);
        var resourceDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Resources.Delete);

        // ResourceSchedule permissions.
        var resourceScheduleReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.ResourceSchedules.Read);
        var resourceScheduleCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.ResourceSchedules.Create);
        var resourceScheduleUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.ResourceSchedules.Update);
        var resourceScheduleDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.ResourceSchedules.Delete);

        // ResourceType permissions.
        var resourceTypeReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.ResourceTypes.Read);
        var resourceTypeCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.ResourceTypes.Create);
        var resourceTypeUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.ResourceTypes.Update);
        var resourceTypeDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.ResourceTypes.Delete);

        // Service permissions.
        var serviceReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Services.Read);
        var serviceCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Services.Create);
        var serviceUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Services.Update);
        var serviceDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Services.Delete);

        // User permissions.
        var userReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Users.Read);
        var userCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Users.Create);
        var userUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Users.Update);
        var userDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Users.Delete);

        // Role permissions.
        var roleReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Roles.Read);
        var roleCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Roles.Create);
        var roleUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Roles.Update);
        var roleDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Roles.Delete);

        // Permission permissions.
        var permissionReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Permissions.Read);
        var permissionCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Permissions.Create);
        var permissionUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Permissions.Update);
        var permissionDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            PermissionNames.Permissions.Delete);

        _permissions =
        [
            appointmentReadPermission.Value!,
            appointmentCreatePermission.Value!,
            appointmentUpdatePermission.Value!,
            appointmentDeletePermission.Value!,

            appointmentStatusReadPermission.Value!,
            appointmentStatusCreatePermission.Value!,
            appointmentStatusUpdatePermission.Value!,
            appointmentStatusDeletePermission.Value!,

            calendarReadPermission.Value!,
            calendarCreatePermission.Value!,
            calendarUpdatePermission.Value!,
            calendarDeletePermission.Value!,

            calendarHolidayReadPermission.Value!,
            calendarHolidayCreatePermission.Value!,
            calendarHolidayUpdatePermission.Value!,
            calendarHolidayDeletePermission.Value!,

            resourceReadPermission.Value!,
            resourceCreatePermission.Value!,
            resourceUpdatePermission.Value!,
            resourceDeletePermission.Value!,

            resourceScheduleReadPermission.Value!,
            resourceScheduleCreatePermission.Value!,
            resourceScheduleUpdatePermission.Value!,
            resourceScheduleDeletePermission.Value!,

            resourceTypeReadPermission.Value!,
            resourceTypeCreatePermission.Value!,
            resourceTypeUpdatePermission.Value!,
            resourceTypeDeletePermission.Value!,

            serviceReadPermission.Value!,
            serviceCreatePermission.Value!,
            serviceUpdatePermission.Value!,
            serviceDeletePermission.Value!,

            userReadPermission.Value!,
            userCreatePermission.Value!,
            userUpdatePermission.Value!,
            userDeletePermission.Value!,

            roleReadPermission.Value!,
            roleCreatePermission.Value!,
            roleUpdatePermission.Value!,
            roleDeletePermission.Value!,

            permissionReadPermission.Value!,
            permissionCreatePermission.Value!,
            permissionUpdatePermission.Value!,
            permissionDeletePermission.Value!
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
        var adminResult = await userManager.CreateUserAsync(
            userId: UserId.Create(),
            email: EmailAddress.From("alice@example.com"),
            passwordHash: PasswordHash.FromHashed(passwordHash),
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
        var managerResult = await userManager.CreateUserAsync(
            UserId.Create(),
            EmailAddress.From("bob@example.com"),
            passwordHash: PasswordHash.FromHashed(passwordHash),
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
        var clientResult = await userManager.CreateUserAsync(
            UserId.Create(),
            EmailAddress.From("carol@example.com"),
            passwordHash: PasswordHash.FromHashed(passwordHash),
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
        var client2Result = await userManager.CreateUserAsync(
            UserId.Create(),
            EmailAddress.From("lexi@example.com"),
            passwordHash: PasswordHash.FromHashed(passwordHash),
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
