using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Calendars.Enums;

/// <summary>
/// Determines the overlapping strategy for appointments.
/// </summary>
public enum AppointmentOverlappingStrategy
{
    [Display(Name = "Allow overlapping")]
    Allow = 1,

    [Display(Name = "Reject overlapping")]
    Reject = 2
}
