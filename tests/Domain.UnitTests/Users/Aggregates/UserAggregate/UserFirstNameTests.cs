using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;

namespace AgendaManager.Domain.UnitTests.Users.Aggregates.UserAggregate;

public class UserFirstNameTests
{
    [Fact]
    public void UserFirstName_ShouldRaiseException_WhenInvalidFirstNameIsSet()
    {
        // Arrange
        var firstName = new string('*', 257);

        // Assert
        Assert.Throws<UserDomainException>(() => UserFactory.CreateUser(firstName: firstName));
    }
}
