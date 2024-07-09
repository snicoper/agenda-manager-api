using AgendaManager.Domain.Users;
using AgendaManager.TestCommon.TestConstants;

namespace AgendaManager.TestCommon.Factories;

public abstract class UserFactory
{
    public static User CreateUser()
    {
        return User.Create(
            Constants.Users.Id,
            Constants.Users.Email,
            Constants.Users.UserName,
            Constants.Users.FirstName,
            Constants.Users.LastName,
            Constants.Users.PasswordHash);
    }
}
