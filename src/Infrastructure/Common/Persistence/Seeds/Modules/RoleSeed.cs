using AgendaManager.Domain.Users.Constants;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Modules;

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
            "Assignable staff role");

        List<Role> roles = [adminRole.Value!, employeeRole.Value!, customerRole.Value!, assignableStaff.Value!];

        foreach (var role in roles.Where(role => context.Roles.All(r => r.Name != role.Name)))
        {
            context.Roles.Add(role);
        }

        await context.SaveChangesAsync();

        return roles;
    }
}
