using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Errors;

public static class UserProfileErrors
{
    public static Error IdentityDocumentAlreadyExists => Error.Conflict(
        "UserProfile.IdentityDocumentAlreadyExists",
        "Identity document already exists.");
}
