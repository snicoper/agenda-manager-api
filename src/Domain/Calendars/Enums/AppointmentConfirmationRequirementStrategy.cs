using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Calendars.Enums;

/// <summary>
/// Determines the creation strategy for appointments.
/// </summary>
public enum AppointmentConfirmationRequirementStrategy
{
    [Display(Name = "Require confirmation")]
    Require = 1,

    [Display(Name = "Auto accept")]
    AutoAccept = 2
}
