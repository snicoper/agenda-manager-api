using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Calendars.Enums;

public enum HolidayStrategy
{
    [Display(Name = "Reject if overlapping")]
    RejectIfOverlapping = 1,

    [Display(Name = "Cancel if overlapping")]
    CancelOverlapping = 2,

    [Display(Name = "Allow overlapping")]
    AllowOverlapping = 3
}
