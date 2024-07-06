using AgendaManager.Domain.Users;
using AgendaManager.TestCommon.TestConstants;

namespace AgendaManager.TestCommon.Users;

public abstract class UserFactory
{
    public static User CreateUser()
    {
        return User.Create(
            Constants.Users.Id,
            Constants.Users.Email,
            Constants.Users.UserName,
            Constants.Users.FirstName,
            Constants.Users.LastName);
    }

    public static List<User> CreateUsers(int number)
    {
        return Enumerable.Range(1, number).Select(_ => CreateUser()).ToList();
    }
}
