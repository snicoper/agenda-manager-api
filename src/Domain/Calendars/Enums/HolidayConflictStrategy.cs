using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Calendars.Enums;

/// <summary>
/// Determines the creation strategy for holidays.
/// </summary>
public enum HolidayConflictStrategy
{
    [Display(Name = "Allow overlapping")]
    Allow = 1,

    [Display(Name = "Reject if overlapping")]
    Reject = 2,

    [Display(Name = "Cancel overlapping")]
    Cancel = 3
}
