using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Modules;

public static class CalendarSeed
{
    public static async Task InitializeAsync(AppDbContext context, IServiceProvider serviceProvider)
    {
        if (context.Calendars.Any())
        {
            return;
        }

        var calendarManager = serviceProvider.GetRequiredService<CalendarManager>();

        var calendarResult = await calendarManager.CreateCalendarAsync(
            calendarId: CalendarId.Create(),
            timeZone: "Europe/Madrid",
            name: "Calendario principal",
            description: "Calendario principal de la aplicación",
            HolidayCreationStrategy.CancelOverlapping,
            cancellationToken: CancellationToken.None);

        if (calendarResult.IsFailure || calendarResult.Value == null)
        {
            throw new Exception("No se pudo crear el seed: Calendario Principal");
        }

        context.Calendars.Add(calendarResult.Value);
        await context.SaveChangesAsync();
    }
}
