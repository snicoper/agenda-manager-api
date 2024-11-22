using AgendaManager.Domain.Common.ValueObjects.ColorScheme;
using AgendaManager.Domain.Common.ValueObjects.Duration;
using AgendaManager.Domain.Services.Services;
using AgendaManager.Domain.Services.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Features;

public static class ServiceSeed
{
    public static async Task InitializeAsync(AppDbContext context, IServiceProvider serviceProvider)
    {
        if (context.Services.Any())
        {
            return;
        }

        var calendar = context.Calendars.FirstOrDefault();
        var resourceType = context.ResourceTypes.ToList();

        if (calendar is null || resourceType.Count == 0)
        {
            throw new Exception("There is a lack of resources to be able to create the seed.");
        }

        var serviceManager = serviceProvider.GetRequiredService<ServiceManager>();

        var service = await serviceManager.CreateServiceAsync(
            ServiceId.Create(),
            calendar.Id,
            Duration.From(TimeSpan.FromMinutes(45)),
            "Service 1",
            "Service 1 description",
            ColorScheme.From("#ffffff", "#000000"));

        if (service.IsFailure)
        {
            throw new Exception($"Failed to create the seed ${service.Error}");
        }

        var odontologo = resourceType.FirstOrDefault(r => r.Name == "Odontólogo")
                         ?? throw new Exception("Resource type not found");

        service.Value?.AddResourceType(odontologo);

        context.Services.Add(service.Value!);

        await context.SaveChangesAsync();
    }
}
