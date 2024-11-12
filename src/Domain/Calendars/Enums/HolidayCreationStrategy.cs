namespace AgendaManager.Domain.Calendars.Enums;

public enum HolidayCreationStrategy
{
    RejectIfOverlapping = 1,
    CancelOverlapping = 2,
    AllowOverlapping = 3
}
