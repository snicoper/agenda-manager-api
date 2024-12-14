using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Users.Enums;

public enum IdentityDocumentType
{
    [Display(Name = "D.N.I.")]
    NationalId = 1,

    [Display(Name = "N.I.E.")]
    ForeignerId = 2,

    [Display(Name = "Passport")]
    Passport = 3,

    [Display(Name = "Residence Permit")]
    ResidencePermit = 4,

    [Display(Name = "Driver's License")]
    DriversLicense = 5,

    [Display(Name = "Health Card")]
    HealthCard = 6,

    [Display(Name = "Social Security Number")]
    SocialSecurityNumber = 7
}
