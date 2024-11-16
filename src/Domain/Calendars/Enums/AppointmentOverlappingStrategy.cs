namespace AgendaManager.Domain.Calendars.Enums;

/// <summary>
/// Como debería comportarse el calendario cuando se intenta crear una cita que se solape
/// con citas ya existentes.
/// </summary>
public enum AppointmentOverlappingStrategy
{
    RejectIfOverlapping = 1,
    AllowOverlapping = 2
}
