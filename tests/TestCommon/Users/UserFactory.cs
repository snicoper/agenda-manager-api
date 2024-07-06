using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Users;

public abstract class UserFactory
{
    public static User CreateUser()
    {
        return User.Create(UserId.Create(), Email.From("test@example.com"), "test", "test", "test");
    }

    public static List<User> CreateUsers(int number)
    {
        return Enumerable.Range(1, number).Select(_ => CreateUser()).ToList();
    }
}
