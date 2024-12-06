using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Errors;

public static class UserProfileErrors
{
    public static Error IdentityDocumentAlreadyExists => Error.Conflict(
        code: "UserProfile.IdentityDocumentAlreadyExists",
        description: "Identity document already exists.");
}
