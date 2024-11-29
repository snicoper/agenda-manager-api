using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AgendaManager.Application.Common.Http.OrderBy;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderType
{
    [Display(Name = "None")]
    None = 0,

    [Display(Name = "ASC")]
    Asc = 1,

    [Display(Name = "DESC")]
    Desc = 2
}
