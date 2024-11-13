using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Resources.Enums;

public enum ResourceCategory
{
    [Display(Name = "None")]
    None = 0,

    [Display(Name = "Staff")]
    Staff = 1,

    [Display(Name = "Facility")]
    Facility = 2,

    [Display(Name = "Equipment")]
    Equipment = 3
}
