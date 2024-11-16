namespace AgendaManager.Domain.Calendars.Enums;

/// <summary>
/// Indica el estado inicial de una cita creada.
/// Si es directa, el cliente no recibe un correo de confirmación.
/// Si requiere confirmación, el cliente recibe un correo de confirmación.
/// </summary>
public enum AppointmentCreationStrategy
{
    Direct = 1,
    RequireConfirmation = 2
}
