using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Resources.Enums;

public enum ResourceType
{
    [Display(Name = "Person")]
    Person = 1,

    [Display(Name = "Place")]
    Place = 2,

    [Display(Name = "Equipment")]
    Equipment = 3
}
