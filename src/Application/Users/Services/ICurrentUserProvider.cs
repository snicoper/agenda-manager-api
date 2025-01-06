using AgendaManager.Application.Users.Models;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Application.Users.Services;

public interface ICurrentUserProvider
{
    public bool IsAuthenticated { get; }

    CurrentUser? GetCurrentUser();

    CalendarId GetSelectedCalendarId();
}
