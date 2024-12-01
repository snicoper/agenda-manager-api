using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Constants;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Features;

public static class UserSeed
{
    public static async Task InitializeAsync(AppDbContext context, IServiceProvider serviceProvider, List<Role> roles)
    {
        if (context.Users.Any())
        {
            return;
        }

        if (roles.Count == 0)
        {
            throw new Exception("Roles not found for create new users.");
        }

        var userManager = serviceProvider.GetRequiredService<UserManager>();
        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();
        var authorizationManager = serviceProvider.GetRequiredService<AuthorizationService>();

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

            var adminRole = roles.First(r => r.Name == SystemRoles.Administrator);
            var employeeRole = roles.First(r => r.Name == SystemRoles.Employee);
            var customerRole = roles.First(r => r.Name == SystemRoles.Customer);

            await authorizationManager.AddRoleToUserAsync(adminResult.Value.Id, adminRole.Id, CancellationToken.None);

            await authorizationManager.AddRoleToUserAsync(
                adminResult.Value.Id,
                employeeRole.Id,
                CancellationToken.None);

            await authorizationManager.AddRoleToUserAsync(
                adminResult.Value.Id,
                customerRole.Id,
                CancellationToken.None);
        }

        // Manager user.
        var managerResult = await userManager.CreateUserAsync(
            UserId.Create(),
            EmailAddress.From("bob@example.com"),
            passwordHash: PasswordHash.FromHashed(passwordHash),
            "Bob",
            "Doe",
            active: true,
            isAssignableResource: false,
            emailConfirmed: true,
            CancellationToken.None);

        if (managerResult.IsFailure || managerResult.Value is null)
        {
            return;
        }

        if (!await context.Users.AnyAsync(u => u.Email.Equals(managerResult.Value.Email)))
        {
            await context.SaveChangesAsync();

            var managerRole = roles.First(r => r.Name == SystemRoles.Administrator);
            var customerRole = roles.First(r => r.Name == SystemRoles.Customer);

            await authorizationManager.AddRoleToUserAsync(
                managerResult.Value.Id,
                managerRole.Id,
                CancellationToken.None);
            await authorizationManager.AddRoleToUserAsync(
                managerResult.Value.Id,
                customerRole.Id,
                CancellationToken.None);
        }

        // Client user.
        var clientResult = await userManager.CreateUserAsync(
            UserId.Create(),
            EmailAddress.From("carol@example.com"),
            passwordHash: PasswordHash.FromHashed(passwordHash),
            "Carol",
            "Doe",
            active: true,
            isAssignableResource: false,
            emailConfirmed: true,
            CancellationToken.None);

        if (clientResult.IsFailure || clientResult.Value is null)
        {
            return;
        }

        if (!await context.Users.AnyAsync(u => u.Email.Equals(clientResult.Value.Email)))
        {
            await context.SaveChangesAsync();

            var customerRole = roles.First(r => r.Name == SystemRoles.Customer);
            await authorizationManager.AddRoleToUserAsync(
                clientResult.Value.Id,
                customerRole.Id,
                CancellationToken.None);
        }

        // Client user.
        var client2Result = await userManager.CreateUserAsync(
            UserId.Create(),
            EmailAddress.From("lexi@example.com"),
            passwordHash: PasswordHash.FromHashed(passwordHash),
            "Lexi",
            "Doe",
            active: true,
            isAssignableResource: false,
            emailConfirmed: false,
            CancellationToken.None);

        if (client2Result.IsFailure || client2Result.Value is null)
        {
            return;
        }

        if (!await context.Users.AnyAsync(u => u.Email.Equals(client2Result.Value.Email)))
        {
            var customerRole = roles.First(r => r.Name == SystemRoles.Customer);
            await authorizationManager.AddRoleToUserAsync(
                client2Result.Value.Id,
                customerRole.Id,
                CancellationToken.None);
        }

        await context.SaveChangesAsync();
    }
}
