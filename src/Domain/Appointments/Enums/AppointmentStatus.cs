using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Appointments.Enums;

public enum AppointmentStatus
{
    [Display(Name = "Pending")]
    Pending = 1,

    [Display(Name = "Accepted")]
    Accepted = 2,

    [Display(Name = "Rejected")]
    Rejected = 3,

    [Display(Name = "Cancelled")]
    Cancelled = 4,

    [Display(Name = "Completed")]
    Completed = 5
}
