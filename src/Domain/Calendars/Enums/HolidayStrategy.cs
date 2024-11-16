namespace AgendaManager.Domain.Calendars.Enums;

public enum HolidayStrategy
{
    RejectIfOverlapping = 1,
    CancelOverlapping = 2,
    AllowOverlapping = 3
}
