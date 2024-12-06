using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Users.Enums;

public enum IdentityDocumentType
{
    [Display(Name = "D.N.I.")]
    NationalId,

    [Display(Name = "N.I.E.")]
    ForeignerId,

    [Display(Name = "Passport")]
    Passport,

    [Display(Name = "Residence Permit")]
    ResidencePermit,

    [Display(Name = "Driver's License")]
    DriversLicense,

    [Display(Name = "Health Card")]
    HealthCard,

    [Display(Name = "Social Security Number")]
    SocialSecurityNumber
}
