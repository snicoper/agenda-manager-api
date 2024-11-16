using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Calendars.Enums;

public enum AppointmentOverlappingStrategy
{
    [Display(Name = "Reject if overlapping")]
    RejectIfOverlapping = 1,

    [Display(Name = "Allow overlapping")]
    AllowOverlapping = 2
}
