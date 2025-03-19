using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources.Services;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Features;

public static class ResourceSeed
{
    public static async Task InitializeAsync(AppDbContext context, IServiceProvider serviceProvider)
    {
        if (context.Resources.Any())
        {
            return;
        }

        var resourceManager = serviceProvider.GetRequiredService<ResourceManager>();

        var user = context.Users.FirstOrDefault(u => u.Email.Equals(EmailAddress.From("alice@example.com")));
        var calendar = context.Calendars.FirstOrDefault();
        var resourceType = context.ResourceTypes.FirstOrDefault();

        if (user is null || calendar is null || resourceType is null)
        {
            throw new Exception("There is a lack of resources to be able to create the seed.");
        }

        var resourceResult = await resourceManager.CreateResourceAsync(
            resourceId: ResourceId.Create(),
            userId: user.Id,
            calendarId: calendar!.Id,
            typeId: resourceType.Id,
            name: "Dr. Smith",
            description: "Dr. Smith is a doctor.",
            colorScheme: ColorScheme.From("#FF0000", "#00FF00"),
            isActive: true,
            cancellationToken: CancellationToken.None);

        if (resourceResult.IsFailure || resourceResult.Value is null)
        {
            throw new Exception("Could not create the resource seed.");
        }

        context.Resources.Add(resourceResult.Value);
        await context.SaveChangesAsync();
    }
}
