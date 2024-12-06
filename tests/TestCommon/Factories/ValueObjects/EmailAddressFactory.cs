using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.TestCommon.Constants;

namespace AgendaManager.TestCommon.Factories.ValueObjects;

public static class EmailAddressFactory
{
    public static EmailAddress Create(string? email = null)
    {
        return EmailAddress.From(email ?? UserConstants.UserAlice.Email.Value);
    }
}
