namespace AgendaManager.Domain.Calendars.Enums;

public enum HolidayCreateStrategy
{
    RejectIfOverlapping = 1,
    CancelOverlapping = 2,
    AllowOverlapping = 3
}
