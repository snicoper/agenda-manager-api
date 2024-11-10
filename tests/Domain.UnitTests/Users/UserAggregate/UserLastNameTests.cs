using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserLastNameTests
{
    [Fact]
    public void UserLastName_ShouldRaiseException_WhenInvalidLastNameIsSet()
    {
        // Arrange
        var lastName = new string('*', 257);

        // Assert
        Assert.Throws<UserDomainException>(() => UserFactory.CreateUser(lastName: lastName));
    }
}
