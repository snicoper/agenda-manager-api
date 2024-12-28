using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.ResourceManagement.Resources.Enums;

public enum ResourceScheduleType
{
    [Display(Name = "Available")]
    Available = 1,

    [Display(Name = "Unavailable")]
    Unavailable = 2
}
