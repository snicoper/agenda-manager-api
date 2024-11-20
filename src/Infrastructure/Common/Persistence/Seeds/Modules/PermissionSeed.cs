using AgendaManager.Domain.Users.Constants;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Modules;

public static class PermissionSeed
{
    public static async Task InitializeAsync(AppDbContext context, IServiceProvider serviceProvider, List<Role> roles)
    {
        if (context.Permissions.Any())
        {
            return;
        }

        if (roles.Count == 0)
        {
            throw new Exception("Roles not found for create new permissions type");
        }

        var permissionManager = serviceProvider.GetRequiredService<PermissionManager>();
        var authorizationManager = serviceProvider.GetRequiredService<AuthorizationService>();

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

        // UserToken permissions.
        var userTokenReadPermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.UsersTokens.Read);
        var userTokenCreatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.UsersTokens.Create);
        var userTokenUpdatePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.UsersTokens.Update);
        var userTokenDeletePermission = await permissionManager.CreatePermissionAsync(
            PermissionId.Create(),
            SystemPermissions.UsersTokens.Delete);

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

        List<Permission> permissions =
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

            userTokenReadPermission.Value!,
            userTokenCreatePermission.Value!,
            userTokenUpdatePermission.Value!,
            userTokenDeletePermission.Value!,

            roleReadPermission.Value!,
            roleCreatePermission.Value!,
            roleUpdatePermission.Value!,
            roleDeletePermission.Value!,

            permissionReadPermission.Value!,
            permissionCreatePermission.Value!,
            permissionUpdatePermission.Value!,
            permissionDeletePermission.Value!
        ];

        foreach (var permission in permissions.Where(
                     permission => context.Permissions.All(p => p.Name != permission.Name)))
        {
            context.Permissions.Add(permission);
        }

        await context.SaveChangesAsync();

        // Asignar todos los permisos a role Administrator.
        var adminRole = roles.First(r => r.Name == SystemRoles.Administrator);
        foreach (var permission in permissions)
        {
            await authorizationManager.AddPermissionToRole(adminRole.Id, permission.Id);
        }

        // Asignar todos permisos a role Employee.
        var managerRole = roles.First(r => r.Name == SystemRoles.Employee);
        foreach (var permission in permissions)
        {
            await authorizationManager.AddPermissionToRole(managerRole.Id, permission.Id);
        }

        // Asignar solo permisos de lectura a role Customer.
        var clientRole = roles.First(r => r.Name == SystemRoles.Customer);
        foreach (var permission in permissions.Where(p => p.Name.Contains("read")))
        {
            await authorizationManager.AddPermissionToRole(clientRole.Id, permission.Id);
        }

        await context.SaveChangesAsync();
    }
}
