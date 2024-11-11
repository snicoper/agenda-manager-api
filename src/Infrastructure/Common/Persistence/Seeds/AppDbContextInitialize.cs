using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Constants;
using AgendaManager.Domain.Users.Entities;
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
    AuthorizationManager authorizationManager,
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

        var adminRole = await roleManager.CreateRoleAsync(
            RoleId.Create(),
            SystemRoles.Administrator,
            "Administrator role");
        var customerRole = await roleManager.CreateRoleAsync(RoleId.Create(), SystemRoles.Customer, "Customer role");
        var employeeRole = await roleManager.CreateRoleAsync(RoleId.Create(), SystemRoles.Employee, "Employee role");
        var assignableStaff = await roleManager.CreateRoleAsync(
            RoleId.Create(),
            SystemRoles.AssignableStaff,
            "Assignable staff role");

        _roles = [adminRole.Value!, employeeRole.Value!, customerRole.Value!, assignableStaff.Value!];

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
            SystemPermissions.Appointments.Read);
        var appointmentCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Appointments.Create);
        var appointmentUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Appointments.Update);
        var appointmentDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Appointments.Delete);

        // AppointmentStatus permissions.
        var appointmentStatusReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.AppointmentStatuses.Read);
        var appointmentStatusCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.AppointmentStatuses.Create);
        var appointmentStatusUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.AppointmentStatuses.Update);
        var appointmentStatusDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.AppointmentStatuses.Delete);

        // Calendar permissions.
        var calendarReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Calendars.Read);
        var calendarCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Calendars.Create);
        var calendarUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Calendars.Update);
        var calendarDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Calendars.Delete);

        // CalendarHoliday permissions.
        var calendarHolidayReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.CalendarHolidays.Read);
        var calendarHolidayCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.CalendarHolidays.Create);
        var calendarHolidayUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.CalendarHolidays.Update);
        var calendarHolidayDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.CalendarHolidays.Delete);

        // Resource permissions.
        var resourceReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Resources.Read);
        var resourceCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Resources.Create);
        var resourceUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Resources.Update);
        var resourceDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Resources.Delete);

        // ResourceSchedule permissions.
        var resourceScheduleReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.ResourceSchedules.Read);
        var resourceScheduleCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.ResourceSchedules.Create);
        var resourceScheduleUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.ResourceSchedules.Update);
        var resourceScheduleDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.ResourceSchedules.Delete);

        // ResourceType permissions.
        var resourceTypeReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.ResourceTypes.Read);
        var resourceTypeCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.ResourceTypes.Create);
        var resourceTypeUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.ResourceTypes.Update);
        var resourceTypeDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.ResourceTypes.Delete);

        // Service permissions.
        var serviceReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Services.Read);
        var serviceCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Services.Create);
        var serviceUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Services.Update);
        var serviceDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Services.Delete);

        // User permissions.
        var userReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Users.Read);
        var userCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Users.Create);
        var userUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Users.Update);
        var userDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Users.Delete);

        // Role permissions.
        var roleReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Roles.Read);
        var roleCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Roles.Create);
        var roleUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Roles.Update);
        var roleDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Roles.Delete);

        // Permission permissions.
        var permissionReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Permissions.Read);
        var permissionCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Permissions.Create);
        var permissionUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Permissions.Update);
        var permissionDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.Permissions.Delete);

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

        // Asignar todos los permisos a role Administrator.
        var adminRole = _roles.First(r => r.Name == SystemRoles.Administrator);
        foreach (var permission in _permissions)
        {
            await authorizationManager.AddPermissionToRole(adminRole.Id, permission.Id);
        }

        // Asignar todos permisos a role Employee.
        var managerRole = _roles.First(r => r.Name == SystemRoles.Employee);
        foreach (var permission in _permissions)
        {
            await authorizationManager.AddPermissionToRole(managerRole.Id, permission.Id);
        }

        // Asignar solo permisos de lectura a role Customer.
        var clientRole = _roles.First(r => r.Name == SystemRoles.Customer);
        foreach (var permission in _permissions.Where(p => p.Name.Contains("read")))
        {
            await authorizationManager.AddPermissionToRole(clientRole.Id, permission.Id);
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

            var adminRole = _roles.First(r => r.Name == SystemRoles.Administrator);
            var employeeRole = _roles.First(r => r.Name == SystemRoles.Employee);
            var customerRole = _roles.First(r => r.Name == SystemRoles.Customer);

            await authorizationManager.AddRoleToUserAsync(adminResult.Value.Id, adminRole.Id);
            await authorizationManager.AddRoleToUserAsync(adminResult.Value.Id, employeeRole.Id);
            await authorizationManager.AddRoleToUserAsync(adminResult.Value.Id, customerRole.Id);
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

            var managerRole = _roles.First(r => r.Name == SystemRoles.Administrator);
            var customerRole = _roles.First(r => r.Name == SystemRoles.Customer);

            await authorizationManager.AddRoleToUserAsync(managerResult.Value.Id, managerRole.Id);
            await authorizationManager.AddRoleToUserAsync(managerResult.Value.Id, customerRole.Id);
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

            var customerRole = _roles.First(r => r.Name == SystemRoles.Customer);
            await authorizationManager.AddRoleToUserAsync(clientResult.Value.Id, customerRole.Id);
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
            var customerRole = _roles.First(r => r.Name == SystemRoles.Customer);
            await authorizationManager.AddRoleToUserAsync(client2Result.Value.Id, customerRole.Id);
        }

        await context.SaveChangesAsync();
    }
}
