using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.ResourceManagement.Shared.Enums;

public enum ResourceCategory
{
    [Display(Name = "Staff")]
    Staff = 1,

    [Display(Name = "Place")]
    Place = 2,

    [Display(Name = "Equipment")]
    Equipment = 3
}
