namespace AgendaManager.Domain.Calendars.Enums;

/// <summary>
/// Estrategias de creación días festivos en el calendario.
/// Como debería comportarse el calendario cuando se intenta crear un día festivo que se solape
/// con citas en el día festivo.
/// </summary>
public enum HolidayCreateStrategy
{
    RejectIfOverlapping = 1,
    CancelOverlapping = 2,
    AllowOverlapping = 3
}
