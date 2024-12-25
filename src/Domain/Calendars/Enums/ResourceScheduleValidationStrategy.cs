using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Calendars.Enums;

/// <summary>
/// Determines the resources schedule validation strategy for appointments.
/// </summary>
public enum ResourceScheduleValidationStrategy
{
    [Display(Name = "Validate schedules")]
    Validate = 1,

    [Display(Name = "Ignore schedules")]
    Ignore = 2
}
