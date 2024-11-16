using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Appointments.Enums;

public enum AppointmentStatus
{
    [Display(Name = "Pending")]
    Pending = 1,

    [Display(Name = "Accepted")]
    Accepted = 2,

    [Display(Name = "Waiting")]
    Waiting = 3,

    [Display(Name = "Cancelled")]
    Cancelled = 4,

    [Display(Name = "RequiresRescheduling")]
    RequiresRescheduling = 5,

    [Display(Name = "InProgress")]
    InProgress = 6,

    [Display(Name = "Completed")]
    Completed = 7
}
