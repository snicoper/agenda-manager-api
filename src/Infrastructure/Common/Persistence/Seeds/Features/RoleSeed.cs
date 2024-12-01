using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Constants;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Features;

public static class RoleSeed
{
    public static async Task<List<Role>> InitializeAsync(AppDbContext context, IServiceProvider serviceProvider)
    {
        if (context.Roles.Any())
        {
            return [];
        }

        var roleManager = serviceProvider.GetRequiredService<RoleManager>();

        var adminRole = await roleManager.CreateRoleAsync(
            RoleId.Create(),
            SystemRoles.Administrator,
            "Administrator role");

        var customerRole = await roleManager.CreateRoleAsync(RoleId.Create(), SystemRoles.Customer, "Customer role");
        var employeeRole = await roleManager.CreateRoleAsync(RoleId.Create(), SystemRoles.Employee, "Employee role");

        var assignableStaff = await roleManager.CreateRoleAsync(
            RoleId.Create(),
            SystemRoles.AssignableStaff,
            "This role is designated for employees who will serve as resources in services, it doesn't require permission assignments");

        List<Role> roles = [adminRole.Value!, employeeRole.Value!, customerRole.Value!, assignableStaff.Value!];

        foreach (var role in roles.Where(role => context.Roles.All(r => r.Name != role.Name)))
        {
            context.Roles.Add(role);
        }

        await context.SaveChangesAsync();

        return roles;
    }
}
