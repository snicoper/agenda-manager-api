using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.ResourceTypes.Enums;

public enum ResourceScheduleType
{
    [Display(Name = "Available")]
    Available = 1,

    [Display(Name = "Unavailable")]
    Unavailable = 2
}
