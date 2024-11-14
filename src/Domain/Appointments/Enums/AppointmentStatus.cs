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
    Cancelled = 5,

    [Display(Name = "RequiresRescheduling")]
    RequiresRescheduling = 6,

    [Display(Name = "InProgress")]
    InProgress = 7,

    [Display(Name = "Completed")]
    Completed = 8
}
